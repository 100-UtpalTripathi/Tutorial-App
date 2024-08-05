using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TutorialApp.Exceptions.Course;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs;
using TutorialApp.Models.DTOs.Course;
using TutorialApp.Services;

namespace TutorialApp.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class CoursesController : ControllerBase
    {

        #region Dependency Injection
        private readonly IAdminService _adminService;
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<CoursesController> _logger;
        public CoursesController(IAdminService adminService, IAzureBlobService azureBlobService, ILogger<CoursesController> logger)
        {
            _adminService = adminService;
            _azureBlobService = azureBlobService;
            _logger = logger;
        }

        #endregion

        #region Get All Courses

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {
                var courses = await _adminService.GetAllCoursesAsync();
                var response = new ApiResponse<IEnumerable<Course>>((int)HttpStatusCode.OK, "Courses retrieved successfully", courses);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        #endregion

        #region Create Course

        [HttpPost("create")]
        public async Task<IActionResult> CreateCourse([FromForm] CourseDTO courseDTO)
        {
            if (courseDTO.Image != null)
            {
                var fileStream = courseDTO.Image.OpenReadStream();
                // Generate a unique file name
                var uniqueFileName = GenerateUniqueFileName(courseDTO.Image.FileName);

                // Upload file with a unique name
                var result = await _azureBlobService.UploadFileAsync("tutorialapp", "CourseImages", uniqueFileName, fileStream);

                if (result.IsError)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Image Upload Failed!", null);
                    return BadRequest(errorResponse);
                }

                courseDTO.CourseImageUrl = result.FileUri.ToString();
            }
            else
            {
                courseDTO.CourseImageUrl = null;
            }

            try
            {
                var response = await _adminService.CreateCourseAsync(courseDTO);
                var successResponse = new ApiResponse<Course>((int)HttpStatusCode.OK, "Course created successfully", response);
                return Ok(successResponse);
            }
            catch (CourseCreationFailedException ex)
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

        #region Update Course

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromForm] CourseDTO courseDTO)
        {
            if (courseDTO.Image != null)
            {
                var fileStream = courseDTO.Image.OpenReadStream();
                // Generate a unique file name
                var uniqueFileName = GenerateUniqueFileName(courseDTO.Image.FileName);

                // Upload file with a unique name
                var result = await _azureBlobService.UploadFileAsync("tutorialapp", "CourseImages", uniqueFileName, fileStream);

                if (result.IsError)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Image Upload Failed!", null);
                    return BadRequest(errorResponse);
                }

                courseDTO.CourseImageUrl = result.FileUri.ToString();
            }
            else
            {
                courseDTO.CourseImageUrl = null;
            }
            try
            {
                var updatedCourse = await _adminService.UpdateCourseAsync(id, courseDTO);
                var response = new ApiResponse<Course>((int)HttpStatusCode.OK, "Course updated successfully", updatedCourse);
                return Ok(response);
            }
            catch (NoSuchCourseFoundException)
            {
                _logger.LogError("Course not found");
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, "Course not found", null);
                return NotFound(errorResponse);
            }
            catch (CourseUpdateFailedException ex)
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

        #region Delete Course

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var deletedCourse = await _adminService.DeleteCourseAsync(id);
                var response = new ApiResponse<Course>((int)HttpStatusCode.OK, "Course deleted successfully", deletedCourse);
                return Ok(response);
            }
            catch (NoSuchCourseFoundException)
            {
                _logger.LogError("Course not found");
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, "Course not found", null);
                return NotFound(errorResponse);
            }
            catch (CourseDeletionFailedException ex)
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


        private string GenerateUniqueFileName(string originalFileName)
        {
            var fileExtension = Path.GetExtension(originalFileName);
            var uniqueFileName = $"{Guid.NewGuid()}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";
            return uniqueFileName;
        }
    }
}
