using StudentInfoSystem.Data.Entities;
using StudentInfoSystem.Models;

namespace StudentInfoSystem.Services
{
    public class UpdatedStudentInfoModel
    {
        public StudentListInfoModel Student { get; set; }
        public ContactEntity Contact { get; set; }
        public ICollection<LessonEntity> Lessons { get; set; }
    }
}
