namespace StudentInfoSystem.Data.Entities
{
    public class StudentLessonEntity
    {
        public int LessonID { get; set; }
        public int StudentId { get; set; }

        public LessonEntity Lesson { get; set; } = null!;
        public StudentEntity Student { get; set; } = null!;
    }
}
