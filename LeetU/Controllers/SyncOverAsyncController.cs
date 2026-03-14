using LeetU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeetU.Controllers;

/// <summary>
/// Контроллер с синхронным вызовом асинхронных операций — риск deadlock в ASP.NET.
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
    /// GET sync/course/{id} — получение курса через .GetAwaiter().GetResult() (sync over async).
    /// В контексте с SynchronizationContext (например UI или старый ASP.NET) может вызвать deadlock.
    /// </summary>
    [HttpGet("course/{courseId:long}")]
    public IActionResult GetCourseSync([FromRoute] long courseId)
    {
        var courses = _courseService.GetCourses(courseId);
        var course = courses.FirstOrDefault();
        // Имитация вызова async: блокирующее ожидание задачи.
        Task.Delay(0).GetAwaiter().GetResult();
        return course == null ? NotFound() : Ok(course);
    }

    /// <summary>
    /// POST sync/course — создание курса через .Result.
    /// SetCourseAsync(...).Result блокирует поток и может привести к deadlock.
    /// </summary>
    [HttpPost("course")]
    public IActionResult CreateCourseSync([FromBody] LeetU.Models.Course course)
    {
        var rows = _courseService.SetCourseAsync(course).Result;
        return rows > 0 ? Ok() : BadRequest();
    }
}
