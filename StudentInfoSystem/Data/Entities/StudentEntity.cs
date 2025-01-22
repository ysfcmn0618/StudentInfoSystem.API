using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace StudentInfoSystem.Data.Entities
{
    public class StudentEntity
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }
        [Required(ErrorMessage = "Öğrencinin adı boş olamaz.")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Öğrencinin Soy adı boş olamaz.")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public DateOnly DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public DateOnly EnrollmentDate { get; set; }
        public bool IsActive { get; set; } = true;

        public string PhotoUrl { get; set; } = string.Empty;
        public int ContactID { get; set; }
        public ContactEntity Contact { get; set; } = null!;
        [JsonIgnore]
        public ICollection<LessonEntity> Lessons { get; set; } = new List<LessonEntity>();
    }
}
