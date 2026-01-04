namespace Archiboard.Api.Models;

public class Database
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public DatabaseTechnology Technology { get; set; }
}

