using LeetU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeetU.Controllers;

/// <summary>
/// Контроллер экспорта данных.
/// </summary>
[ApiController]
[Route("export")]
public class ExportController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ICourseService _courseService;

    public ExportController(IStudentService studentService, ICourseService courseService)
    {
        _studentService = studentService;
        _courseService = courseService;
    }

    /// <summary>
    /// GET export/students — получение списка студентов без sync-over-async.
    /// </summary>
    [HttpGet("students")]
    public IActionResult ExportStudents()
    {
        var students = _studentService.GetStudents().ToList();
        return Ok(students);
    }

    /// <summary>
    /// POST export/assign/{studentId}/{courseId} — полностью асинхронное назначение студента на курс.
    /// </summary>
    [HttpPost("assign/{studentId:long}/{courseId:long}")]
    public async Task<IActionResult> AssignCourseAsync([FromRoute] long studentId, [FromRoute] long courseId)
    {
        var rowsAffected = await _studentService.SetStudentCourseAsync(studentId, courseId);
        return Ok(rowsAffected);
    }
}