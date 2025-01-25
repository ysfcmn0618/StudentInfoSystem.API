using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentInfoSystem.Data;
using StudentInfoSystem.Models;

namespace StudentInfoSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StudentController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudent()
        {
            var studentInfos = await _context.Students
                .Select(x => new StudentListInfoModel
                {
                    StudentId = x.StudentId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DateOfBirth = x.DateOfBirth,
                    Gender = x.Gender,
                    GPA = x.Lessons != null && x.Lessons.Any() ? x.Lessons.Average(a => a.GPA) : 0,
                    PhotoUrl = x.PhotoUrl,
                })
                .ToListAsync();
            return Ok(studentInfos);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var student = await _context.Students.SingleOrDefaultAsync(x => x.StudentId == id);
            if (student is null)
            {
                return NotFound();
            }
            var studentContact = await _context.Contacts.SingleOrDefaultAsync(x => x.StudentId == id);
            _context.Students.Remove(student);
            if (studentContact is not null)
            {
                _context.Contacts.Remove(studentContact);
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Öğrenci başarıyla silindi." });
        }


    }
}
