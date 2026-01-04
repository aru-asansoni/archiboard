using Archiboard.Api.Models;
using Archiboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Archiboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrokersController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public BrokersController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet]
    public ActionResult<List<Broker>> GetBrokers()
    {
        return Ok(_dataStore.GetBrokers());
    }

    [HttpGet("{id}")]
    public ActionResult<Broker> GetBroker(Guid id)
    {
        var broker = _dataStore.GetBroker(id);
        if (broker == null) return NotFound();
        return Ok(broker);
    }

    [HttpPost]
    public ActionResult<Broker> CreateBroker([FromBody] Broker broker)
    {
        var created = _dataStore.CreateBroker(broker);
        return CreatedAtAction(nameof(GetBroker), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<Broker> UpdateBroker(Guid id, [FromBody] Broker broker)
    {
        var updated = _dataStore.UpdateBroker(id, broker);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBroker(Guid id)
    {
        if (!_dataStore.DeleteBroker(id)) return NotFound();
        return NoContent();
    }
}

