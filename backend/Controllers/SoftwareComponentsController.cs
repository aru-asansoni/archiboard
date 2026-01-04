using Archiboard.Api.Models;
using Archiboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Archiboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SoftwareComponentsController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public SoftwareComponentsController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet]
    public ActionResult<List<SoftwareComponent>> GetSoftwareComponents()
    {
        return Ok(_dataStore.GetSoftwareComponents());
    }

    [HttpGet("{id}")]
    public ActionResult<SoftwareComponent> GetSoftwareComponent(Guid id)
    {
        var component = _dataStore.GetSoftwareComponent(id);
        if (component == null) return NotFound();
        return Ok(component);
    }

    [HttpPost]
    public ActionResult<SoftwareComponent> CreateSoftwareComponent([FromBody] SoftwareComponent component)
    {
        var created = _dataStore.CreateSoftwareComponent(component);
        return CreatedAtAction(nameof(GetSoftwareComponent), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<SoftwareComponent> UpdateSoftwareComponent(Guid id, [FromBody] SoftwareComponent component)
    {
        var updated = _dataStore.UpdateSoftwareComponent(id, component);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteSoftwareComponent(Guid id)
    {
        if (!_dataStore.DeleteSoftwareComponent(id)) return NotFound();
        return NoContent();
    }
}

