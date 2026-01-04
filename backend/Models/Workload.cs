namespace Archiboard.Api.Models;

public class Workload
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RepoUrl { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public List<Guid> SoftwareComponentIds { get; set; } = new();
    public List<SoftwareComponent> SoftwareComponents { get; set; } = new();
    public Guid? RuntimeId { get; set; }
    public Runtime? Runtime { get; set; }
    public List<Guid> APIsExposedIds { get; set; } = new();
    public List<ApiDefinition> APIsExposed { get; set; } = new();
    public List<Guid> APIsInvokedIds { get; set; } = new();
    public List<ApiDefinition> APIsInvoked { get; set; } = new();
    public List<Guid> ConsumeMessageFromIds { get; set; } = new();
    public List<MessageConnection> ConsumeMessageFrom { get; set; } = new();
    public List<Guid> ProduceMessageToIds { get; set; } = new();
    public List<MessageConnection> ProduceMessageTo { get; set; } = new();
    public List<Guid> DatabaseIds { get; set; } = new();
    public List<Database> Databases { get; set; } = new();
}

