using LeetU.Data.Entities;

namespace LeetU.Data.Interfaces;

//public interface ICourseRepository : IRepositoryCrud<Course>   //old vers.
//{
//}
/*public interface ICourseRepository
{
    IEnumerable<Course> GetCourses(params long[] courseIds);

    IEnumerable<long> GetAllCourseIds();

    IEnumerable<CourseWithStudentCountEntity> GetCoursesWithStudentCount(int skip, int take);

    int GetCoursesCount();
}*/
public interface ICourseRepository : IRepositoryCrud<Course>
{
    IEnumerable<CourseWithStudentCountEntity> GetCoursesWithStudentCount(int skip, int take);

    int GetCoursesCount();
}