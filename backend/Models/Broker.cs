namespace Archiboard.Api.Models;

public class Broker
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public BrokerType Type { get; set; }
    public string ClusterName { get; set; } = string.Empty;
}

