namespace StudentInfoSystem.Models
{
    public class StudentListInfoModel
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public double GPA { get; set; }
    }
}
