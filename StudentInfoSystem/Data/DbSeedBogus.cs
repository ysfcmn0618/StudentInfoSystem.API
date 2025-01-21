using Bogus;
using Microsoft.EntityFrameworkCore;
using StudentInfoSystem.Data.Entities;

namespace StudentInfoSystem.Data
{
    public  class DbSeedBogus
    {
        public static void SeedDatabase(AppDbContext context)
        {
            if (context.Students.Any() || context.Contacts.Any() || context.Lessons.Any())
                return; // Eğer veritabanında veri varsa ekleme yapma
          
            var lessonNames = new[] { "Mathematics", "Physics", "Chemistry", "History", "Biology","Computer Engineering","Art" };
            var lessonFaker = new Faker<LessonEntity>()
                .RuleFor(l => l.LessonName, f => f.PickRandom(lessonNames)) // Belirli ders isimlerinden birini seçer
                .RuleFor(l => l.EnrollmentDateClass, f => DateOnly.FromDateTime(f.Date.Past(2)))
                .RuleFor(l => l.GradeLevel, f => f.Random.Int(1, 12).ToString())
                .RuleFor(l => l.GPA, f => f.Random.Float(1.0f, 4.0f))
                .RuleFor(l => l.LessonTeacherName, f => f.Name.FullName());


            var lessons = lessonFaker.Generate(5); // 5 sahte ders oluştur

            context.Lessons.AddRange(lessons);

            var contactFaker = new Faker<ContactEntity>()
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.Address, f => f.Address.FullAddress())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.ParentName, f => f.Name.FullName())
                .RuleFor(c => c.ParentContact, f => f.Phone.PhoneNumber());

            var studentFaker = new Faker<StudentEntity>()
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.DateOfBirth, f => DateOnly.FromDateTime(f.Date.Past(20, DateTime.Now.AddYears(-5))))
                .RuleFor(s => s.Gender, f => f.PickRandom("Male", "Female"))
                .RuleFor(s => s.EnrollmentDate, f => DateOnly.FromDateTime(f.Date.Past(2)))
                .RuleFor(s => s.IsActive, f => f.Random.Bool())
                .RuleFor(s => s.PhotoUrl, f => f.Internet.Avatar())
                .RuleFor(s => s.ContactID, f => 0) // İlişki kurulduktan sonra düzenlenecek
                .RuleFor(s => s.Lessons, f => new List<LessonEntity>(f.PickRandom(lessons, 2)));

            var contacts = new List<ContactEntity>();
            var students = studentFaker.Generate(20); // 20 sahte öğrenci oluştur

            foreach (var student in students)
            {
                var contact = contactFaker.Generate();
                contacts.Add(contact);
                student.ContactID = contact.ContactId; // İlişkiyi kur
            }

            context.Contacts.AddRange(contacts);
            context.Students.AddRange(students);

            context.SaveChanges(); // Değişiklikleri kaydet
        }


    }

}

