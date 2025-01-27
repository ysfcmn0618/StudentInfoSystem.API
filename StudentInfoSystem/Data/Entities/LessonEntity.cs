using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudentInfoSystem.Data.Entities
{
    public class LessonEntity
    {
        [Key]
        public int LessonID { get; set; }
        public string LessonName { get; set; } = string.Empty;        
        public DateOnly EnrollmentDateClass { get; set; }
        public string GradeLevel { get; set; } = string.Empty;
        public double GPA { get; set; }
        public string LessonTeacherName { get; set; } = string.Empty;
        [JsonIgnore]
        public  ICollection<StudentLessonEntity> Students { get; set; }= new List<StudentLessonEntity>();
    }
}
