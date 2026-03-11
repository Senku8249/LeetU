using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetU.Data.Entities
{
    public class CourseWithStudentCountEntity
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int StudentCount { get; set; }
    }
}
