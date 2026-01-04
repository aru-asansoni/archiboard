using Archiboard.Api.Models;
using Archiboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Archiboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApisController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public ApisController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet]
    public ActionResult<List<ApiDefinition>> GetApis()
    {
        return Ok(_dataStore.GetApis());
    }

    [HttpGet("{id}")]
    public ActionResult<ApiDefinition> GetApi(Guid id)
    {
        var api = _dataStore.GetApi(id);
        if (api == null) return NotFound();
        return Ok(api);
    }

    [HttpPost]
    public ActionResult<ApiDefinition> CreateApi([FromBody] ApiDefinition api)
    {
        var created = _dataStore.CreateApi(api);
        return CreatedAtAction(nameof(GetApi), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<ApiDefinition> UpdateApi(Guid id, [FromBody] ApiDefinition api)
    {
        var updated = _dataStore.UpdateApi(id, api);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteApi(Guid id)
    {
        if (!_dataStore.DeleteApi(id)) return NotFound();
        return NoContent();
    }
}

