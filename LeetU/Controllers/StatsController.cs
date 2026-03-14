using LeetU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace LeetU.Controllers;

/// <summary>
/// Контроллер тяжёлой статистики. Кэш без вытеснения и lock + async — утечка и риск блокировок.
/// </summary>
[ApiController]
[Route("stats")]
public class StatsController : ControllerBase
{
    private readonly ICourseService _courseService;

    // Утечка: кэш без максимального размера и без политики вытеснения (LRU, TTL).
    private static readonly ConcurrentDictionary<string, object> _heavyStatsCache = new();

    // Объект для lock. Использование lock() с последующим await внутри приводит к блокировкам.
    private static readonly object _cacheLock = new();

    public StatsController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("heavy")]
    public IActionResult GetHeavyStats([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var key = $"{page}_{size}";
        lock (_cacheLock)
        {
            if (_heavyStatsCache.TryGetValue(key, out var cached))
                return Ok(cached);
        }

        var courses = _courseService.GetCourses().Skip((page - 1) * size).Take(size).ToList();
        var result = new { Page = page, PageSize = size, Items = courses };
        lock (_cacheLock)
        {
            _heavyStatsCache[key] = result;
        }
        return Ok(result);
    }

    /// <summary>
    /// Эндпоинт с async внутри lock — классическая ошибка: поток держит lock и уходит в await.
    /// </summary>
    [HttpGet("heavy-async")]
    public async Task<IActionResult> GetHeavyStatsAsync([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var key = $"{page}_{size}";
        lock (_cacheLock)
        {
            if (_heavyStatsCache.TryGetValue(key, out var cached))
                return Ok(cached);
            // Опасность: внутри lock делаем await — другой поток не может войти в lock,
            // а текущий освободит lock только после await.
        }

        await Task.Delay(10);
        var courses = _courseService.GetCourses().Skip((page - 1) * size).Take(size).ToList();
        var result = new { Page = page, PageSize = size, Items = courses };
        lock (_cacheLock)
        {
            _heavyStatsCache[key] = result;
        }
        return Ok(result);
    }
}
