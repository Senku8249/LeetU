using LeetU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LeetU.Controllers;

/// <summary>
/// Контроллер тяжёлой статистики с кэшем, у которого есть TTL и ограничение размера.
/// </summary>
[ApiController]
[Route("stats")]
public class StatsController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly IMemoryCache _cache;

    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public StatsController(ICourseService courseService, IMemoryCache cache)
    {
        _courseService = courseService;
        _cache = cache;
    }

    [HttpGet("heavy")]
    public IActionResult GetHeavyStats([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        if (page <= 0 || size <= 0)
            return BadRequest("page и size должны быть положительными числами");

        var key = $"heavy:{page}:{size}";

        if (!_cache.TryGetValue(key, out object? result))
        {
            var courses = _courseService
                .GetCourses()
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            result = new
            {
                Page = page,
                PageSize = size,
                Items = courses
            };

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheTtl)
                .SetSize(1);

            _cache.Set(key, result, cacheOptions);
        }

        return Ok(result);
    }

    [HttpGet("heavy-async")]
    public async Task<IActionResult> GetHeavyStatsAsync([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        if (page <= 0 || size <= 0)
            return BadRequest("page и size должны быть положительными числами");

        var key = $"heavy-async:{page}:{size}";

        if (!_cache.TryGetValue(key, out object? result))
        {
            await Task.Delay(10);

            var courses = _courseService
                .GetCourses()
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            result = new
            {
                Page = page,
                PageSize = size,
                Items = courses
            };

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheTtl)
                .SetSize(1);

            _cache.Set(key, result, cacheOptions);
        }

        return Ok(result);
    }
}