using LeetU.Models;
using LeetU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeetU.Controllers;

/// <summary>
/// The course controller. All controllers should have MINIMAL code (why do you think minimal apis exist). We take the request and then just perform an action on the service
/// Having minimal code reduces the need to test logic in these controllers, which is pretty tricky.
/// We COULD use MediatR pattern here, but in this example we are just keeping things simple. Google MediatR if you're curious
/// </summary>
[ApiController]
[Route("course")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [Route("")]
    [HttpGet]
    public IActionResult GetAll()
    {
        var courses = _courseService.GetCourses();
        return new OkObjectResult(courses);
    }

    [Route("{courseId}")]
    [HttpGet]
    public IActionResult GetCourse([FromRoute] long courseId)
    {
        var courses = _courseService.GetCourses(courseId);
        var course = courses.FirstOrDefault();

        return course == null ? new NotFoundResult() : new OkObjectResult(course);
    }

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> SetCourse([FromBody] Course course)
    {
        var rowsAffected = await _courseService.SetCourseAsync(course);

        if (rowsAffected == 0)
            return BadRequest("'message': 'Не удалось создать курс'");

        //throw new Exception("There was an error creating the course");
        return new OkResult();
    }

    // GET api/courses/ids
    [HttpGet("ids")]
    public IActionResult GetCourseIds()
    {
        var ids = _courseService.GetAllCourseIds();
        return Ok(ids);
    }
    [HttpGet("with-stats")]//10.03 контроллер курсов
    public IActionResult GetCoursesWithStats([FromQuery] int page, [FromQuery] int pageSize)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("page и pageSize должны быть положительными числами");
        }

        var result = _courseService.GetCoursesWithStats(page, pageSize);
        return Ok(result);
    }
}