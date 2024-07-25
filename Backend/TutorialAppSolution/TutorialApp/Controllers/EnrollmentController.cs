using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.Enrollment;
using TutorialApp.Models;

namespace TutorialApp.Controllers
{
    [Route("api/user/course/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<Enrollment>> EnrollCourse(EnrollmentDTO enrollmentDTO)
        {
            var enrollment = await _enrollmentService.EnrollCourseAsync(enrollmentDTO);
            return CreatedAtAction(nameof(GetEnrolledCoursesByUser), new { userEmail = enrollment.UserEmail }, enrollment);
        }

        [HttpGet("get/{userEmail}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetEnrolledCoursesByUser(string userEmail)
        {
            var courses = await _enrollmentService.GetEnrolledCoursesByUserAsync(userEmail);
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }
            return Ok(courses);
        }

        [HttpPut("status")]
        public async Task<ActionResult<Enrollment>> UpdateEnrollmentStatus(EnrollmentDTO enrollmentDTO)
        {
            try
            {
                var enrollment = await _enrollmentService.UpdateEnrollmentStatusAsync(enrollmentDTO);
                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
