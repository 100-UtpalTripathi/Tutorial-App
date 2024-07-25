using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Exceptions.Course;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.Course;
using TutorialApp.Models;

namespace TutorialApp.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public CoursesController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
            var courses = await _adminService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Course>> CreateCourse([FromForm] CourseDTO courseDTO)
        {
            if (courseDTO == null)
            {
                return BadRequest();
            }

            var response = await _adminService.CreateCourseAsync(courseDTO);
            return Ok(response);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<Course>> UpdateCourse(int id, [FromBody] CourseDTO courseDTO)
        {
            
            try
            {
                var updatedCourse = await _adminService.UpdateCourseAsync(id, courseDTO);
                return Ok(updatedCourse);
            }
            catch (NoSuchCourseFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            try
            {
                var deletedCourse = await _adminService.DeleteCourseAsync(id);
                return Ok(deletedCourse);
            }
            catch (NoSuchCourseFoundException)
            {
                return NotFound();
            }
        }
    }
}
