namespace Archiboard.Api.Models;

public class Topic
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid BrokerId { get; set; }
    public Broker? Broker { get; set; }
    public int NumberOfPartitions { get; set; }
}

