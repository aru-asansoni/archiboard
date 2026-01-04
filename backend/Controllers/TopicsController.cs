using Archiboard.Api.Models;
using Archiboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Archiboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicsController : ControllerBase
{
    private readonly IDataStore _dataStore;

    public TopicsController(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    [HttpGet]
    public ActionResult<List<Topic>> GetTopics()
    {
        return Ok(_dataStore.GetTopics());
    }

    [HttpGet("{id}")]
    public ActionResult<Topic> GetTopic(Guid id)
    {
        var topic = _dataStore.GetTopic(id);
        if (topic == null) return NotFound();
        return Ok(topic);
    }

    [HttpPost]
    public ActionResult<Topic> CreateTopic([FromBody] Topic topic)
    {
        var created = _dataStore.CreateTopic(topic);
        return CreatedAtAction(nameof(GetTopic), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<Topic> UpdateTopic(Guid id, [FromBody] Topic topic)
    {
        var updated = _dataStore.UpdateTopic(id, topic);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTopic(Guid id)
    {
        if (!_dataStore.DeleteTopic(id)) return NotFound();
        return NoContent();
    }
}

