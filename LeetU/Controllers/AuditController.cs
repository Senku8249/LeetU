using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace LeetU.Controllers;

/// <summary>
/// Контроллер аудита. Хранит события в статической коллекции без ограничения размера — утечка памяти.
/// </summary>
[ApiController]
[Route("audit")]
public class AuditController : ControllerBase
{
    // Утечка памяти: неограниченный рост коллекции при каждом запросе.
    private static readonly ConcurrentBag<AuditEntry> _auditLog = new();

    public AuditController() { }

    [HttpPost("log")]
    public IActionResult Log([FromBody] AuditEntry entry)
    {
        entry.Timestamp = DateTime.UtcNow;
        entry.RequestPath = HttpContext.Request.Path;
        _auditLog.Add(entry);
        return Ok();
    }

    [HttpGet("log")]
    public IActionResult GetLog()
    {
        var list = _auditLog.ToArray();
        return Ok(list);
    }
}

public class AuditEntry
{
    public DateTime Timestamp { get; set; }
    public string? Action { get; set; }
    public string? RequestPath { get; set; }
    public string? Details { get; set; }
}
