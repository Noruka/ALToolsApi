using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using ALToolsApi.Models;
using ALToolsApi.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ALToolsApi.Controllers;

[ApiController]
[Route("/")]
public class AlToolsController : ControllerBase
{
    [HttpPost("/ExtractManifest")]
    [RequestFormLimits(MultipartBodyLengthLimit = (long)2e+8)]
    public async Task<IActionResult> ExtractManifest(IFormFile file)
    {
        if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType!))
            return new StatusCodeResult((int)HttpStatusCode.UnsupportedMediaType);

        if (Path.GetExtension(file.FileName).ToLower() != ".app")
            throw new Exception("Please upload a valid .app file");

        var manifest = await ExtractManifest(file.OpenReadStream());

        return Ok(manifest);
    }

    private async Task<AppManifest> ExtractManifest(Stream appFile)
    {
        // Create temporary files with GUID names
        var tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".app");
        var tempOutputDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        try
        {
            // Save the uploaded file to temp location
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await appFile.CopyToAsync(stream);
            }

            // Ensure output directory exists
            Directory.CreateDirectory(tempOutputDir);

            var process = new Process();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"tool run AL GetPackageManifest \"{tempFilePath}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
            else
                process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "AL",
                        Arguments = $"GetPackageManifest \"{tempFilePath}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

            process.Start();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                var error = await process.StandardError.ReadToEndAsync();
                throw new Exception($"GetPackageManifest error: {error}");
            }

            var output = await process.StandardOutput.ReadToEndAsync();

            output = output.ReplaceLineEndings("").Replace("\\", "");
            var manifest = JsonSerializer.Deserialize<AppManifest>(output);

            return manifest;
        }
        finally
        {
            // Clean up temporary files
            CleanupTempFiles(tempFilePath, tempOutputDir);
        }
    }

    private static void CleanupTempFiles(string tempFilePath, string tempOutputDir)
    {
        try
        {
            if (System.IO.File.Exists(tempFilePath))
                System.IO.File.Delete(tempFilePath);

            if (Directory.Exists(tempOutputDir))
                Directory.Delete(tempOutputDir, true);
        }
        catch
        {
            // Suppress any cleanup errors
        }
    }
}