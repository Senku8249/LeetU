using LeetU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeetU.Controllers;

/// <summary>
/// Контроллер без блокирующих sync-over-async вызовов.
/// </summary>
[ApiController]
[Route("sync")]
public class SyncOverAsyncController : ControllerBase
{
    private readonly ICourseService _courseService;

    public SyncOverAsyncController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    /// <summary>
    /// GET sync/course/{id} — получение курса без блокирующего ожидания.
    /// </summary>
    [HttpGet("course/{courseId:long}")]
    public IActionResult GetCourse([FromRoute] long courseId)
    {
        var courses = _courseService.GetCourses(courseId);
        var course = courses.FirstOrDefault();

        return course == null ? NotFound() : Ok(course);
    }

    /// <summary>
    /// POST sync/course — полностью асинхронное создание курса через await.
    /// </summary>
    [HttpPost("course")]
    public async Task<IActionResult> CreateCourseAsync([FromBody] LeetU.Models.Course course)
    {
        var rows = await _courseService.SetCourseAsync(course);
        return rows > 0 ? Ok() : BadRequest();
    }
}