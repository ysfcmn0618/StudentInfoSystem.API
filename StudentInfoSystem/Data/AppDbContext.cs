using Microsoft.EntityFrameworkCore;
using StudentInfoSystem.Data.Entities;

namespace StudentInfoSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<LessonEntity> Lessons { get; set; }
        public DbSet<StudentLessonEntity> StudentLessons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentLessonEntity>()
           .HasKey(ls => new { ls.LessonID, ls.StudentId });

            // StudentLessonEntity - LessonEntity ilişkisi
            modelBuilder.Entity<StudentLessonEntity>()
                .HasOne(ls => ls.Lesson)
                .WithMany(l => l.Students)
                .HasForeignKey(ls => ls.LessonID);

            // StudentLessonEntity - StudentEntity ilişkisi
            modelBuilder.Entity<StudentLessonEntity>()
                .HasOne(ls => ls.Student)
                .WithMany(s => s.Lessons)
                .HasForeignKey(ls => ls.StudentId);
            // Student - Contact ilişkisini tanımlıyoruz (One-to-One)
            modelBuilder.Entity<StudentEntity>()
                .HasOne(s => s.Contact)
                .WithMany()
                .HasForeignKey(s => s.ContactId);
        }
       
    }

}
