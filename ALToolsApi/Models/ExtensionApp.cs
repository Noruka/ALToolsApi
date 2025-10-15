using System.Text.Json.Serialization;

namespace ALToolsApi.Models;

public class AppManifest
{
    [JsonPropertyName("id")] public Guid Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("publisher")] public string Publisher { get; set; }

    [JsonPropertyName("brief")] public string Brief { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("version")] public string Version { get; set; }

    [JsonPropertyName("privacyStatement")] public Uri PrivacyStatement { get; set; }

    [JsonPropertyName("EULA")] public Uri Eula { get; set; }

    [JsonPropertyName("help")] public Uri Help { get; set; }

    [JsonPropertyName("url")] public Uri Url { get; set; }

    [JsonPropertyName("logo")] public string Logo { get; set; }

    [JsonPropertyName("runtime")] public string Runtime { get; set; }

    [JsonPropertyName("showMyCode")] public bool ShowMyCode { get; set; }

    [JsonPropertyName("target")] public string Target { get; set; }

    [JsonPropertyName("application")] public string Application { get; set; }

    [JsonPropertyName("platform")] public string Platform { get; set; }

    [JsonPropertyName("contextSensitiveHelpUrl")]
    public Uri ContextSensitiveHelpUrl { get; set; }

    [JsonPropertyName("features")] public List<string> Features { get; set; }

    [JsonPropertyName("dependencies")] public List<Dependency> Dependencies { get; set; }

    [JsonPropertyName("idRanges")] public List<IdRange> IdRanges { get; set; }

    [JsonPropertyName("build")] public Build Build { get; set; }
}

public class Build
{
    [JsonPropertyName("by")] public string By { get; set; }

    [JsonPropertyName("timestamp")] public DateTimeOffset Timestamp { get; set; }

    [JsonPropertyName("compilerVersion")] public string CompilerVersion { get; set; }
}

public class Dependency
{
    [JsonPropertyName("id")] public Guid Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("version")] public string Version { get; set; }

    [JsonPropertyName("publisher")] public string Publisher { get; set; }
    [JsonPropertyName("dependencies")] public List<Dependency>? Dependencies { get; set; }
}

public class IdRange
{
    [JsonPropertyName("from")] public long From { get; set; }

    [JsonPropertyName("to")] public long To { get; set; }
}