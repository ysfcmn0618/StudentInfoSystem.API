using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentInfoSystem.Models;

namespace StudentInfoSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private static readonly List<Student> StudentsList = new();

        [HttpGet("AllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Student>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAllStudents()
        {
            if (StudentsList is null || StudentsList.Count == 0) return NotFound("Kayıtlı Öğrenci Bulunamadı");
            return Ok(StudentsList);
        }

        [HttpGet("getStudent/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetStudentById([FromRoute] int id)
        {
            var student = StudentsList.FirstOrDefault(s => s.StudentId == id);
            if (student == null) return NotFound("Öğrenci bulunamadı.");
            return Ok(student);
        }
        [HttpPost("AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddStudent([FromForm] Student student)
        {

            var std = StudentsList.FirstOrDefault(x => x.Phone == student.Phone);//Aynı Öğrenciden başka varmı? kontrolü için iki öğrenvi aynı telefon numarasını kullanmamalı diye düşünüyoruz.!
            if (std is not null) return StatusCode(StatusCodes.Status406NotAcceptable, "Bu telefon numarasıyla başka bir öğrenci kaydedilmiş.");
            student.StudentId = StudentsList.Any() ? StudentsList.Max(s => s.StudentId) + 1 : 1;
            StudentsList.Add(student);
            return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentId }, student);

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}

            //StudentsList.Add(student);  
            //return Ok(student);
        }
        [HttpPut("UpdateStudent/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult PutStudent(int id, [FromBody] Student student)
        {
            var existingStudent = StudentsList.FirstOrDefault(x => x.StudentId == id);
            if (existingStudent is null)
            {
                return NotFound($"Öğrenci ID {id} bulunamadı.");
            }
            var samePhoneStudent= StudentsList.Where(x => x.Phone == student.Phone);
            if (samePhoneStudent.Count() > 1)
            {
                return NotFound("Bu Telefon Numarası Başka Bir Kullanıcıya Ait");
            }

            // Öğrenciyi güncelleme
            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Phone = student.Phone;
            existingStudent.Email = student.Email;
            existingStudent.DateOfBirth = student.DateOfBirth;
            existingStudent.Gender = student.Gender; existingStudent.Address = student.Address;
            existingStudent.EnrollmentDate = student.EnrollmentDate;
            existingStudent.GradeLevel = student.GradeLevel; existingStudent.GPA = student.GPA;
            existingStudent.Courses = student.Courses;
            existingStudent.IsActive = student.IsActive;
            existingStudent.ParentName = student.ParentName;
            existingStudent.ParentContact = student.ParentContact;
            existingStudent.PhotoUrl = student.PhotoUrl;
            return Ok(existingStudent);
        }
        [HttpDelete("DeleteStudent/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult DeleteStudent([FromRoute] int id)
        {
            var crush = StudentsList.FirstOrDefault(x => x.StudentId == id);
            if (crush is null)
            {
                return NotFound($"ID {id} ile bir öğrenci bulunamadı.");
            }

            StudentsList.Remove(crush);
            return Ok(crush);
        }

    }
}
