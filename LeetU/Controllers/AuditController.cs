using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LeetU.Controllers;

/// <summary>
/// Контроллер аудита. Хранит ограниченное количество последних событий в памяти.
/// </summary>
[ApiController]
[Route("audit")]
public class AuditController : ControllerBase
{
    private static readonly Queue<AuditEntry> AuditLog = new Queue<AuditEntry>();
    private static readonly object SyncRoot = new object();
    private const int MaxEntries = 1000;

    public AuditController()
    {
    }

    [HttpPost("log")]
    public IActionResult Log([FromBody] AuditEntry entry)
    {
        entry.Timestamp = DateTime.UtcNow;
        entry.RequestPath = HttpContext.Request.Path;

        lock (SyncRoot)
        {
            if (AuditLog.Count >= MaxEntries)
            {
                AuditLog.Dequeue();
            }

            AuditLog.Enqueue(entry);
        }

        return Ok();
    }

    [HttpGet("log")]
    public IActionResult GetLog()
    {
        AuditEntry[] list;

        lock (SyncRoot)
        {
            list = AuditLog.ToArray();
        }

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