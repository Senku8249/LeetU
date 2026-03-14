using LeetU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeetU.Controllers;

/// <summary>
/// Контроллер экспорта данных. Синхронный вызов асинхронного кода приводит к возможной блокировке.
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
    /// GET export/students — синхронная обёртка над асинхронной операцией через .Result.
    /// В контексте ASP.NET может привести к deadlock (sync over async).
    /// </summary>
    [HttpGet("students")]
    public IActionResult ExportStudents()
    {
        var students = _studentService.GetStudents().ToList();
        // Имитация вызова async-метода синхронно (типичная ошибка).
        var _ = Task.Run(async () =>
        {
            await Task.Delay(1);
            return 0;
        }).Result;
        return Ok(students);
    }

    /// <summary>
    /// POST export/assign — назначает студента на курс через .Result.
    /// Вызов SetStudentCourseAsync(...).Result в потоке с SynchronizationContext приводит к deadlock.
    /// </summary>
    [HttpPost("assign/{studentId:long}/{courseId:long}")]
    public IActionResult AssignCourseSync([FromRoute] long studentId, [FromRoute] long courseId)
    {
        var rowsAffected = _studentService.SetStudentCourseAsync(studentId, courseId).Result;
        return Ok(rowsAffected);
    }
}
