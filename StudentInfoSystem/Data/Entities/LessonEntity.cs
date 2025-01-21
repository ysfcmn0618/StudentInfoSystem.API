using System.ComponentModel.DataAnnotations;

namespace StudentInfoSystem.Data.Entities
{
    public class LessonEntity
    {
        public int LessonID { get; set; }
        public string LessonName { get; set; } = string.Empty;        
        public DateOnly EnrollmentDateClass { get; set; }
        public string GradeLevel { get; set; } = string.Empty;
        public float GPA { get; set; }
        public string LessonTeacherName { get; set; } = string.Empty;
        public ICollection<StudentEntity> Students { get; set; }=new List<StudentEntity>();
    }
}
