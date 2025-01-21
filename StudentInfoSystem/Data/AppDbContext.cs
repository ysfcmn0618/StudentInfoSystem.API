using Microsoft.EntityFrameworkCore;
using StudentInfoSystem.Data.Entities;

namespace StudentInfoSystem.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            
        }
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<LessonEntity> Lessons { get; set; }
    }
}
