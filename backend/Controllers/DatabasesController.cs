using Archiboard.Api.Models;
using Archiboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Archiboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabasesController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public DatabasesController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet]
    public ActionResult<List<Database>> GetDatabases()
    {
        return Ok(_dataStore.GetDatabases());
    }

    [HttpGet("{id}")]
    public ActionResult<Database> GetDatabase(Guid id)
    {
        var database = _dataStore.GetDatabase(id);
        if (database == null) return NotFound();
        return Ok(database);
    }

    [HttpPost]
    public ActionResult<Database> CreateDatabase([FromBody] Database database)
    {
        var created = _dataStore.CreateDatabase(database);
        return CreatedAtAction(nameof(GetDatabase), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<Database> UpdateDatabase(Guid id, [FromBody] Database database)
    {
        var updated = _dataStore.UpdateDatabase(id, database);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDatabase(Guid id)
    {
        if (!_dataStore.DeleteDatabase(id)) return NotFound();
        return NoContent();
    }
}

