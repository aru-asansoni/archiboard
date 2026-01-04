using Archiboard.Api.Models;
using Archiboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Archiboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConnectionsController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public ConnectionsController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet]
    public ActionResult<List<Connection>> GetConnections()
    {
        return Ok(_dataStore.GetConnections());
    }

    [HttpGet("{id}")]
    public ActionResult<Connection> GetConnection(Guid id)
    {
        var connection = _dataStore.GetConnection(id);
        if (connection == null) return NotFound();
        return Ok(connection);
    }

    [HttpPost]
    public ActionResult<Connection> CreateConnection([FromBody] Connection connection)
    {
        var created = _dataStore.CreateConnection(connection);
        return CreatedAtAction(nameof(GetConnection), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<Connection> UpdateConnection(Guid id, [FromBody] Connection connection)
    {
        var updated = _dataStore.UpdateConnection(id, connection);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteConnection(Guid id)
    {
        if (!_dataStore.DeleteConnection(id)) return NotFound();
        return NoContent();
    }
}

