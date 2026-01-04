using Archiboard.Api.Models;
using Archiboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Archiboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RuntimesController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public RuntimesController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet]
    public ActionResult<List<Runtime>> GetRuntimes()
    {
        return Ok(_dataStore.GetRuntimes());
    }

    [HttpGet("{id}")]
    public ActionResult<Runtime> GetRuntime(Guid id)
    {
        var runtime = _dataStore.GetRuntime(id);
        if (runtime == null) return NotFound();
        return Ok(runtime);
    }

    [HttpPost]
    public ActionResult<Runtime> CreateRuntime([FromBody] Runtime runtime)
    {
        var created = _dataStore.CreateRuntime(runtime);
        return CreatedAtAction(nameof(GetRuntime), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<Runtime> UpdateRuntime(Guid id, [FromBody] Runtime runtime)
    {
        var updated = _dataStore.UpdateRuntime(id, runtime);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRuntime(Guid id)
    {
        if (!_dataStore.DeleteRuntime(id)) return NotFound();
        return NoContent();
    }
}

