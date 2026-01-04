using Archiboard.Api.Models;
using Archiboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Archiboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkloadsController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public WorkloadsController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet]
    public ActionResult<List<Workload>> GetWorkloads()
    {
        return Ok(_dataStore.GetWorkloads());
    }

    [HttpGet("{id}")]
    public ActionResult<Workload> GetWorkload(Guid id)
    {
        var workload = _dataStore.GetWorkload(id);
        if (workload == null) return NotFound();
        return Ok(workload);
    }

    [HttpPost]
    public ActionResult<Workload> CreateWorkload([FromBody] Workload workload)
    {
        var created = _dataStore.CreateWorkload(workload);
        return CreatedAtAction(nameof(GetWorkload), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<Workload> UpdateWorkload(Guid id, [FromBody] Workload workload)
    {
        var updated = _dataStore.UpdateWorkload(id, workload);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteWorkload(Guid id)
    {
        if (!_dataStore.DeleteWorkload(id)) return NotFound();
        return NoContent();
    }
}

