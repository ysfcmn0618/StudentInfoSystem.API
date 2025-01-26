using StudentInfoSystem.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudentInfoSystem.Models
{
    public class AddStudentInfoModel
    {
        //public int StudentId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string PhotoUrl { get; set; } = string.Empty;
        //public int ContactId { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
        public string ParentContact { get; set; } = string.Empty;
      //  public int LessonID { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public DateTime EnrollmentDateClass { get; set; }
        public string GradeLevel { get; set; } = string.Empty;
        public double GPA { get; set; }
        public string LessonTeacherName { get; set; } = string.Empty;
        
    }
}
