namespace Archiboard.Api.Models;

public class Runtime
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Vendor { get; set; } = string.Empty;
    public bool LTS { get; set; }
    public DateTime? EOL { get; set; }
}

