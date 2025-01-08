using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace StudentInfoSystem.Models
{
    public class Student
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        public int StudentId { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }
        [Required]
        [MaxLength(300)]
        public string Address { get; set; }
        [Required]
        public DateTime EnrollmentDate { get; set; }
        public string GradeLevel { get; set; }
        public float GPA { get; set; }
        public List<string> Courses { get; set; } = new List<string>();
        public bool IsActive { get; set; }
        public string ParentName { get; set; }
        public string ParentContact { get; set; }
        public string PhotoUrl { get; set; }
        public Student()
        {
            EnrollmentDate = DateTime.Now; // Varsayılan olarak şu anki tarihi ayarlar.
        }
    }
}
