using AutoMapper;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentInfoSystem.Data;
using StudentInfoSystem.Data.Entities;
using StudentInfoSystem.Mapping;
using StudentInfoSystem.Models;

namespace StudentInfoSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StudentController(AppDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudent()
        {
            //var studentInfos = await _context.Students
            //    .Select(x => new StudentListInfoModel
            //    {
            //        StudentId = x.StudentId,
            //        FirstName = x.FirstName,
            //        LastName = x.LastName,
            //        DateOfBirth = x.DateOfBirth,
            //        Gender = x.Gender,
            //        GPA = x.Lessons != null && x.Lessons.Any() ? x.Lessons.Average(a => a.Lesson.GPA) : 0,
            //        PhotoUrl = x.PhotoUrl,
            //    })
            //    .ToListAsync();
            var studentListDb = await _context.Students
                 .Include(x => x.Contact) 
                    .ThenInclude(l => l.Lesson) // Lesson içeriğine erişim için ekleme yapın
                    .ToListAsync();

            // AutoMapper ile listeyi haritalandırın
            var studentModelList = _mapper.Map<List<StudentListInfoModel>>(studentListDb);

            return Ok(studentModelList);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var student = await _context.Students.SingleOrDefaultAsync(x => x.StudentId == id);
            if (student is null)
            {
                return NotFound();
            }
            var studentContact = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactId == student.ContactId);
            _context.Students.Remove(student);
            if (studentContact is not null)
            {
                _context.Contacts.Remove(studentContact);
            }
            var lessons = await _context.StudentLessons.Where(l => l.StudentId == id).ToListAsync();
            if (lessons.Any())
            {
                // İlgili kayıtları sil
                _context.StudentLessons.RemoveRange(lessons);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Öğrenci başarıyla silindi." });
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent([FromForm] AddStudentInfoModel student)
        {
            //var dbStudent = await _context.Students.LastAsync();

            //if (await _context.Students.AnyAsync(s => s. == student.StudentId))
            //    return BadRequest("Böyle bir öğrenci mevcut");

            /*var newStudent = new StudentEntity()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                EnrollmentDate = DateTime.Now,
                IsActive = student.IsActive,
                PhotoUrl = student.PhotoUrl,
                ContactID = student.ContactId,
            };
            var newContact = new ContactEntity()
            {
                Phone = student.Phone,
                Address = student.Address,
                Email = student.Email,
                ParentName = student.ParentName,
                ParentContact = student.ParentContact,
                StudentId = student.StudentId,
            };
            var newLesson = new LessonEntity()
            {
                LessonName = student.LessonName,
                EnrollmentDateClass = DateTime.Now,
                LessonTeacherName = student.LessonTeacherName,
                GPA = student.GPA,
                GradeLevel = student.GradeLevel, 
                StudentId = student.StudentId
            };*/


            var newContact = _mapper.Map<ContactEntity>(student);
            var newLesson = _mapper.Map<LessonEntity>(student);
            var newStudent = _mapper.Map<StudentEntity>(student);
            newStudent.EnrollmentDate = DateOnly.FromDateTime(DateTime.Now);
            newLesson.EnrollmentDateClass = DateOnly.FromDateTime(DateTime.Now);


            //newContact.StudentId = newStudent.StudentId;
            //newLesson. = newStudent.StudentId;
            var contactRes = _context.Contacts.Add(newContact);
            await _context.SaveChangesAsync();

            newStudent.ContactId = newContact.ContactId;
            var studentRes = _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            // newContact.Student = studentRes.Entity;
            // newContact.StudentId= studentRes.Entity.StudentId;
            //// İlişkili Contact ve Lesson kayıtlarını StudentId ile bağlama
            //newContact.StudentId = newStudent.StudentId;
            //newLesson.Students = newStudent.StudentId;
            var lessonRes = _context.Lessons.Add(newLesson);
            await _context.SaveChangesAsync();
            newLesson.LessonID = lessonRes.Entity.LessonID;

            StudentLessonEntity studentLessonEntity = new StudentLessonEntity();
            studentLessonEntity.StudentId = newStudent.StudentId;
            studentLessonEntity.LessonID = newLesson.LessonID;

            _context.StudentLessons.Add(studentLessonEntity);
            await _context.SaveChangesAsync();

            //await _context.Database.BeginTransactionAsync();
            return Ok(new
            {
                Message = "Student successfully added.",
                Student = newStudent,
                Contact = newContact,
                Lesson = newLesson
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent([FromForm] AddStudentInfoModel student,int id )
        {
            // Mevcut öğrenci, iletişim ve ders bilgilerini kontrol et
            var existingStudent = await _context.Students.SingleOrDefaultAsync(s => s.StudentId == id);
            if (existingStudent is null)
            {
                return NotFound(new { Message = "Öğrenci bulunamadı." });
            }

            var existingContact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactId == existingStudent.ContactId);
            if (existingContact is null)
            {
                return NotFound(new { Message = "İletişim bilgileri bulunamadı." });
            }

            var existingLessons = await _context.StudentLessons
                .Where(sl => sl.StudentId == existingStudent.StudentId)
                .ToListAsync();

            // Mevcut verileri güncelle
            var updatedContact = _mapper.Map(student, existingContact); // Var olan iletişim bilgilerini güncelle
            var updatedStudent = _mapper.Map(student, existingStudent); // Var olan öğrenci bilgilerini güncelle
            updatedStudent.ContactId = existingContact.ContactId;

            _context.Contacts.Update(updatedContact);
            _context.Students.Update(updatedStudent);

            // Yeni ders bilgileri oluştur ve güncelle
            var updatedLesson = _mapper.Map<LessonEntity>(student);
            _context.Lessons.Update(updatedLesson);

            await _context.SaveChangesAsync();

            // Ders ile öğrenci ilişkisini güncelle
            if (existingLessons.Any())
            {
                _context.StudentLessons.RemoveRange(existingLessons); // Eski ilişkileri kaldır
            }

            var studentLessonEntity = new StudentLessonEntity
            {
                StudentId = updatedStudent.StudentId,
                LessonID = updatedLesson.LessonID
            };

            _context.StudentLessons.Add(studentLessonEntity);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Öğrenci bilgileri başarıyla güncellendi.",
                Student = updatedStudent,
                Contact = updatedContact,
                Lesson = updatedLesson
            });
        }


    }
}
