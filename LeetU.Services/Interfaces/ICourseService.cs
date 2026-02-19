using LeetU.Models;

namespace LeetU.Services.Interfaces;

public interface ICourseService
{
    //предыдущая реализация
    //  IEnumerable<Course> GetCourses(params long[] courseIds);
    // Task<int> SetCourseAsync(Course course);
    //конец предыдущей реализации
    IEnumerable<Course> GetCourses(params long[] courseIds);

    IEnumerable<long> GetAllCourseIds();

    Task<int> SetCourseAsync(Course course);
    Task<int> UpdateCourseAsync(Course course);
    Task<int> DeleteCourseAsync(long courseId);
}
