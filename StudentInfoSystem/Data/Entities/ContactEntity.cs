using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentInfoSystem.Data.Entities
{
    public class ContactEntity
    {
        public int ContactId { get; set; }
        [Required]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
        public string Phone { get; set; } = string.Empty;
        [Required]
        [MaxLength(300)]
        public string Address { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
        public string ParentContact { get; set; } = string.Empty;
        [ForeignKey(nameof(StudentId))]
        public int  StudentId { get; set; }
        public StudentEntity Student { get; set; } = null!;
    }
}
