using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs;
using TutorialApp.Models.DTOs.Enrollment;
using TutorialApp.Exceptions.Enrollment;
using System.Net;
using Microsoft.AspNetCore.Cors;

namespace TutorialApp.Controllers
{
    [Route("api/user/course/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]

    public class EnrollmentController : ControllerBase
    {
        #region Dependency Injection
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(IEnrollmentService enrollmentService, ILogger<EnrollmentController> logger)
        {
            _enrollmentService = enrollmentService;
            _logger = logger;
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
                _logger.LogError(ex, ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null);
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                    return Ok(errorResponse);
                }
                var response = new ApiResponse<IEnumerable<EnrolledCoursesDTO>>((int)HttpStatusCode.OK, "Enrolled courses retrieved successfully", courses);
                return Ok(response);
            }
            catch (NoSuchEnrollmentFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, ex.Message, null);
                return Ok(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                _logger.LogError(ex, ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, ex.Message, null);
                return NotFound(errorResponse);
            }
            catch (EnrollmentFailedException ex)
            {
                _logger.LogError(ex, ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null);
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        #endregion

        // Get all enrolled courses
        [HttpGet("get")]
        public async Task<IActionResult> GetAllEnrolledCourses()
        {
            try
            {
                var courses = await _enrollmentService.GetAllEnrolledCoursesAsync();
                if (courses == null || !courses.Any())
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, "No enrolled courses found", null);
                    return Ok(errorResponse);
                }
                var response = new ApiResponse<IEnumerable<EnrolledCoursesDTO>>((int)HttpStatusCode.OK, "Enrolled courses retrieved successfully", courses);
                return Ok(response);
            }
            catch (NoSuchEnrollmentFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, ex.Message, null);
                return Ok(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }   
    }
}
