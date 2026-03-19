using LeetU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeetU.Controllers;

/// <summary>
/// Контроллер отчётов. Отдаёт снимки данных по студентам и курсам.
/// </summary>
[ApiController]
[Route("report")]
public class ReportController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ICourseService _courseService;

    public ReportController(IStudentService studentService, ICourseService courseService)
    {
        _studentService = studentService;
        _courseService = courseService;
    }

    /// <summary>
    /// GET report/students/snapshot — возвращает снимок всех id студентов.
    /// </summary>
    [HttpGet("students/snapshot")]
    public IActionResult GetStudentsSnapshot()
    {
        var ids = _studentService.GetAllStudentIds().ToList();
        var snapshot = new
        {
            Timestamp = DateTime.UtcNow,
            StudentIds = ids,
            Count = ids.Count
        };

        return Ok(snapshot);
    }

    /// <summary>
    /// GET report/courses/snapshot — возвращает снимок всех id курсов.
    /// </summary>
    [HttpGet("courses/snapshot")]
    public IActionResult GetCoursesSnapshot()
    {
        var ids = _courseService.GetAllCourseIds().ToList();
        var snapshot = new
        {
            Timestamp = DateTime.UtcNow,
            CourseIds = ids,
            Count = ids.Count
        };

        return Ok(snapshot);
    }
}