using LeetU.Data.Context;
using LeetU.Data.Entities;
using LeetU.Data.Interfaces;

namespace LeetU.Data.Repositories
{
    /// <summary>
    /// WHY do you ask. WHY is this empty? Well, if there any repository operations that are specialised, they go here and not in the CRUD base
    /// For example, if using Stored Procedures, the code to access them would be here.
    /// </summary>
    /*public class CourseRepository : RepositoryCrud<Course>, ICourseRepository
    {
        public CourseRepository(StudentContext context) : base(context)
        {
        }
    }*/
    public class CourseRepository : RepositoryCrud<Course>, ICourseRepository
    {
        private readonly StudentContext _context;

        public CourseRepository(StudentContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<CourseWithStudentCountEntity> GetCoursesWithStudentCount(int skip, int take)
        {
            return _context.Courses
                .OrderBy(course => course.Id)
                .Skip(skip)
                .Take(take)
                .Select(course => new CourseWithStudentCountEntity
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    StudentCount = _context.StudentCourses.Count(sc => sc.CourseId == course.Id)
                })
                .ToList();
        }

        public int GetCoursesCount()
        {
            return _context.Courses.Count();
        }
    }
}
