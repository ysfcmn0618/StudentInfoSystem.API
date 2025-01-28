using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentInfoSystem.Controllers;
using StudentInfoSystem.Data;
using StudentInfoSystem.Data.Entities;
using StudentInfoSystem.Models;

namespace StudentInfoSystem.Services
{
    public class StudentService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public StudentService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<ContactEntity> AddContactAsync(AddStudentInfoModel student)
        {
            var newContact = _mapper.Map<ContactEntity>(student);
            var result = _appDbContext.Contacts.Add(newContact);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<StudentEntity> AddStudentAsync(AddStudentInfoModel student, int contactId)
        {
            var newStudent = _mapper.Map<StudentEntity>(student);
            newStudent.EnrollmentDate = GetCurrentDate();
            newStudent.ContactId = contactId;

            var result = _appDbContext.Students.Add(newStudent);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<LessonEntity> AddLessonAsync(AddStudentInfoModel student)
        {
            var newLesson = _mapper.Map<LessonEntity>(student);
            newLesson.EnrollmentDateClass = GetCurrentDate();

            var result = _appDbContext.Lessons.Add(newLesson);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }


        public async Task AddStudentLessonAsync(int studentId, int lessonId)
        {
            var studentLesson = new StudentLessonEntity
            {
                StudentId = studentId,
                LessonID = lessonId
            };

            _appDbContext.StudentLessons.Add(studentLesson);
            await _appDbContext.SaveChangesAsync();
        }

        private DateOnly GetCurrentDate()
        {
            return DateOnly.FromDateTime(DateTime.Now);
        }

        public async Task<List<StudentListInfoModel>> GetStudentListInfo()
        {
            var studentListDb = await _appDbContext.Students
                 .Include(x => x.Contact)
                    .ToListAsync();

            var studentModelList = _mapper.Map<List<StudentListInfoModel>>(studentListDb);
            return studentModelList;
        }
        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var student = await _appDbContext.Students
                .Include(s => s.Contact) // İlgili Contact bilgisi için Include kullanıyoruz
                .Include(s => s.Lessons) // İlgili ders bağlantıları için Include
                .SingleOrDefaultAsync(x => x.StudentId == studentId);

            if (student is null)
            {
                return false;
            }
            // İlgili StudentLessons kayıtlarını sil
            if (student.Lessons.Any())
            {
                _appDbContext.StudentLessons.RemoveRange(student.Lessons);
            }
            // İlgili Contact kaydını sil
            if (student.Contact is not null)
            {
                _appDbContext.Contacts.Remove(student.Contact);
            }
            // Öğrenci kaydını sil
            _appDbContext.Students.Remove(student);

            // Tüm değişiklikleri kaydet
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        private async Task UpdateLessonsAsync(AddStudentInfoModel student, ICollection<StudentLessonEntity> lessons)
        {
            foreach (var lesson in lessons)
            {
                var existingLesson = await _appDbContext.Lessons.SingleOrDefaultAsync(c => c.LessonID == lesson.LessonID);

                if (existingLesson is null)
                {
                    throw new KeyNotFoundException($"Ders kaydı bulunamadı. LessonID: {lesson.LessonID}");
                }

                _mapper.Map(student, existingLesson);
            }

            // Tüm dersler güncellendikten sonra değişiklikleri kaydet
            await _appDbContext.SaveChangesAsync();
        }
        private  async Task UpdateContactAsync(AddStudentInfoModel student, int contactId)
        {
            var existingContact = await _appDbContext.Contacts.SingleOrDefaultAsync(c => c.ContactId == contactId);

            if (existingContact is null)
            {
                throw new KeyNotFoundException("İletişim kaydı bulunamadı.");
            }

            _mapper.Map(student, existingContact);

            // Değişiklikleri kaydet
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<UpdatedStudentInfoModel> UpdateStudentAsync(AddStudentInfoModel student, int studentId)
        {
            var existingStudent = await _appDbContext.Students
                .Include(s => s.Contact)
                .Include(s => s.Lessons)
                .SingleOrDefaultAsync(c => c.StudentId == studentId);

            if (existingStudent is null)
            {
                throw new KeyNotFoundException("Öğrenci kaydı bulunamadı.");
            }

            // Öğrenci bilgilerini güncelle
            _mapper.Map(student, existingStudent);

            // İletişim bilgilerini güncelle
            await UpdateContactAsync(student, existingStudent.ContactId);

            // Ders bilgilerini güncelle
            await UpdateLessonsAsync(student, existingStudent.Lessons);

            // Tüm değişiklikleri kaydet
            await _appDbContext.SaveChangesAsync();
            var updatedStudentInfo = new UpdatedStudentInfoModel
            {
                Student = _mapper.Map<StudentListInfoModel>(existingStudent),
                Contact = existingStudent.Contact,
                Lessons = existingStudent.Lessons
                    .Select(sl => sl.Lesson) // Derslerin sadece LessonEntity kısmını al
                    .ToList()
            };

            return updatedStudentInfo;
        }

    }
}
