using Archiboard.Api.Models;
using Archiboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Archiboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublishersController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public PublishersController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet]
    public ActionResult<List<Publisher>> GetPublishers()
    {
        return Ok(_dataStore.GetPublishers());
    }

    [HttpGet("{id}")]
    public ActionResult<Publisher> GetPublisher(Guid id)
    {
        var publisher = _dataStore.GetPublisher(id);
        if (publisher == null) return NotFound();
        return Ok(publisher);
    }

    [HttpPost]
    public ActionResult<Publisher> CreatePublisher([FromBody] Publisher publisher)
    {
        var created = _dataStore.CreatePublisher(publisher);
        return CreatedAtAction(nameof(GetPublisher), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<Publisher> UpdatePublisher(Guid id, [FromBody] Publisher publisher)
    {
        var updated = _dataStore.UpdatePublisher(id, publisher);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeletePublisher(Guid id)
    {
        if (!_dataStore.DeletePublisher(id)) return NotFound();
        return NoContent();
    }
}

