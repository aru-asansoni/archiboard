namespace Archiboard.Api.Models;

public class MessageSchema
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public MessageFormat Format { get; set; }
    public MessageType Type { get; set; }
}

