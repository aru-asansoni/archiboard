using Archiboard.Api.Models;
using Archiboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Archiboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageSchemasController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public MessageSchemasController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet]
    public ActionResult<List<MessageSchema>> GetMessageSchemas()
    {
        return Ok(_dataStore.GetMessageSchemas());
    }

    [HttpGet("{id}")]
    public ActionResult<MessageSchema> GetMessageSchema(Guid id)
    {
        var schema = _dataStore.GetMessageSchema(id);
        if (schema == null) return NotFound();
        return Ok(schema);
    }

    [HttpPost]
    public ActionResult<MessageSchema> CreateMessageSchema([FromBody] MessageSchema schema)
    {
        var created = _dataStore.CreateMessageSchema(schema);
        return CreatedAtAction(nameof(GetMessageSchema), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<MessageSchema> UpdateMessageSchema(Guid id, [FromBody] MessageSchema schema)
    {
        var updated = _dataStore.UpdateMessageSchema(id, schema);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMessageSchema(Guid id)
    {
        if (!_dataStore.DeleteMessageSchema(id)) return NotFound();
        return NoContent();
    }
}

