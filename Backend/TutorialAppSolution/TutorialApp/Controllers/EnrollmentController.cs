using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs;
using TutorialApp.Models.DTOs.Enrollment;
using TutorialApp.Exceptions.Enrollment;
using System.Net;

namespace TutorialApp.Controllers
{
    [Route("api/user/course/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        #region Dependency Injection
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }
        #endregion

        #region Add Enrollment

        [HttpPost("add")]
        public async Task<IActionResult> EnrollCourse([FromBody] EnrollmentDTO enrollmentDTO)
        {
            try
            {
                var enrollment = await _enrollmentService.EnrollCourseAsync(enrollmentDTO);
                var response = new ApiResponse<Enrollment>((int)HttpStatusCode.OK, "Course enrollment successful", enrollment);
                return Ok(response);
            }
            catch (EnrollmentFailedException ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null);
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        #endregion


        #region Get Enrolled Courses By User

        [HttpGet("get/{userEmail}")]
        public async Task<IActionResult> GetEnrolledCoursesByUser(string userEmail)
        {
            try
            {
                var courses = await _enrollmentService.GetEnrolledCoursesByUserAsync(userEmail);
                if (courses == null || !courses.Any())
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, "No enrolled courses found", null);
                    return NotFound(errorResponse);
                }
                var response = new ApiResponse<IEnumerable<Course>>((int)HttpStatusCode.OK, "Enrolled courses retrieved successfully", courses);
                return Ok(response);
            }
            catch (NoSuchEnrollmentFoundException ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, ex.Message, null);
                return NotFound(errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        #endregion

        #region Update Enrollment Status

        [HttpPut("status")]
        public async Task<IActionResult> UpdateEnrollmentStatus([FromBody] EnrollmentDTO enrollmentDTO)
        {
            try
            {
                var enrollment = await _enrollmentService.UpdateEnrollmentStatusAsync(enrollmentDTO);
                var response = new ApiResponse<Enrollment>((int)HttpStatusCode.OK, "Enrollment status updated successfully", enrollment);
                return Ok(response);
            }
            catch (NoSuchEnrollmentFoundException ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, ex.Message, null);
                return NotFound(errorResponse);
            }
            catch (EnrollmentFailedException ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null);
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        #endregion
    }
}
