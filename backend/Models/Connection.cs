namespace Archiboard.Api.Models;

public class Connection
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Cardinality Cardinality { get; set; }
    public Guid FromId { get; set; }
    public Guid ToId { get; set; }
}

