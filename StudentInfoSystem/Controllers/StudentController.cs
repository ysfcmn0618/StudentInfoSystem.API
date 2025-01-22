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
            var studentInfos= await _context.Students.Select(x=> new List<StudentInfoModel>
            {
               new StudentInfoModel
               {
                   StudentId=x.StudentId,
                   FirstName=x.FirstName,
                   LastName=x.LastName,
                   DateOfBirth=x.DateOfBirth,
                   Gender=x.Gender,
                   GPA=x.Lessons.Average(a=>a.GPA),
                   PhotoUrl=x.PhotoUrl,
               }
            }).ToListAsync();
            return Ok(studentInfos);
        }


    }
}
