using ContentDispositionHeaderValue = System.Net.Http.Headers.ContentDispositionHeaderValue;

namespace ALToolsApi.Utils;

public static class MultipartRequestHelper
{
    public static bool IsMultipartContentType(string contentType)
    {
        return !string.IsNullOrWhiteSpace(contentType)
               && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
    }
}