namespace Archiboard.Api.Models;

public class ApiDefinition
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty; // SemVer
    public ApiType Type { get; set; }
    public string ServiceUrl { get; set; } = string.Empty;
    public string SpecUrl { get; set; } = string.Empty;
    public SpecType SpecType { get; set; }
    public Exposure Exposure { get; set; }
}

