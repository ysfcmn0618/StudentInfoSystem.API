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
         .Include(x => x.Lessons) // GPA hesaplamak için Lessons tablosunu dahil edin
         .ThenInclude(l => l.Lesson) // Lesson içeriğine erişim için ekleme yapın
         .ToListAsync();

            // AutoMapper ile listeyi haritalandırın
            var studentModelList = _mapper.Map<List<StudentListInfoModel>>(studentListDb);

            return Ok(studentModelList);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            //var student = await _context.Students.SingleOrDefaultAsync(x => x.StudentId == id);
            //if (student is null)
            //{
            //    return NotFound();
            //}
            //var studentContact = await _context.Contacts.SingleOrDefaultAsync(x => x.StudentId == id);
            //_context.Students.Remove(student);
            //if (studentContact is not null)
            //{
            //    _context.Contacts.Remove(studentContact);
            //}
            //await _context.SaveChangesAsync();
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
            newStudent.EnrollmentDate = DateTime.Now;
            newLesson.EnrollmentDateClass = DateTime.Now;

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


    }
}
