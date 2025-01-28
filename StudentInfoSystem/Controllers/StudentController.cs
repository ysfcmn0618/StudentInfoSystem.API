using AutoMapper;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentInfoSystem.Data;
using StudentInfoSystem.Data.Entities;
using StudentInfoSystem.Mapping;
using StudentInfoSystem.Models;
using StudentInfoSystem.Services;

namespace StudentInfoSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("/AllStudents")]
        public async Task<IActionResult> GetAllStudent()
        {
            var result = await _studentService.GetStudentListInfo();

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {

            return await _studentService.DeleteStudentAsync(id) ?
                Ok(new { Message = "Kayıt başarıyla silindi." }) :
                NotFound(new { Message = "Beklenmeyen bir hata oluştu" });
        }
        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent([FromForm] AddStudentInfoModel student)
        {

            var newContact = await _studentService.AddContactAsync(student);

            var newStudent = await _studentService.AddStudentAsync(student, newContact.ContactId);

            var newLesson = await _studentService.AddLessonAsync(student);


            await _studentService.AddStudentLessonAsync(newStudent.StudentId, newLesson.LessonID);

            return Ok(new
            {
                Message = "Student successfully added.",
                Student = newStudent,
                Contact = newContact,
                Lesson = newLesson
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent([FromBody] AddStudentInfoModel student, int id)
        {
            var updatedStudentInfo = await _studentService.UpdateStudentAsync(student, id);

            return Ok(new
            {
                Message = "Öğrenci bilgileri başarıyla güncellendi.",
                Student = updatedStudentInfo.Student,
                Contact = updatedStudentInfo.Contact,
                Lessons = updatedStudentInfo.Lessons
            });
        }


    }
}
