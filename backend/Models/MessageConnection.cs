namespace Archiboard.Api.Models;

public class MessageConnection
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public Topic? Topic { get; set; }
    public List<Guid> MessageSchemaIds { get; set; } = new();
    public List<MessageSchema> Messages { get; set; } = new();
}

