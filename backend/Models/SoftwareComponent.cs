namespace Archiboard.Api.Models;

public class SoftwareComponent
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RepoUrl { get; set; } = string.Empty;
    public Guid? PublisherId { get; set; }
    public Publisher? PublishedBy { get; set; }
    public SoftwareComponentType Type { get; set; }
    public string Version { get; set; } = string.Empty;
    public Language Language { get; set; }
    public List<Guid> ComponentIds { get; set; } = new();
    public List<SoftwareComponent> Components { get; set; } = new();
}

