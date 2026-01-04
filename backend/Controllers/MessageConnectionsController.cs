using Archiboard.Api.Models;
using Archiboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Archiboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageConnectionsController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public MessageConnectionsController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet]
    public ActionResult<List<MessageConnection>> GetMessageConnections()
    {
        return Ok(_dataStore.GetMessageConnections());
    }

    [HttpGet("{id}")]
    public ActionResult<MessageConnection> GetMessageConnection(Guid id)
    {
        var connection = _dataStore.GetMessageConnection(id);
        if (connection == null) return NotFound();
        return Ok(connection);
    }

    [HttpPost]
    public ActionResult<MessageConnection> CreateMessageConnection([FromBody] MessageConnection connection)
    {
        var created = _dataStore.CreateMessageConnection(connection);
        return CreatedAtAction(nameof(GetMessageConnection), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<MessageConnection> UpdateMessageConnection(Guid id, [FromBody] MessageConnection connection)
    {
        var updated = _dataStore.UpdateMessageConnection(id, connection);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMessageConnection(Guid id)
    {
        if (!_dataStore.DeleteMessageConnection(id)) return NotFound();
        return NoContent();
    }
}

