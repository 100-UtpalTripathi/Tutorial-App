using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Exceptions.Course;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.Course;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.User;
using TutorialApp.Services;

namespace TutorialApp.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IAzureBlobService _azureBlobService;
        public CoursesController(IAdminService adminService, IAzureBlobService azureBlobService)
        {
            _adminService = adminService;
            _azureBlobService = azureBlobService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
            var courses = await _adminService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Course>> CreateCourse([FromBody] CourseDTO courseDTO)
        {
            if (courseDTO.Image != null)
            {
                var fileStream = courseDTO.Image.OpenReadStream();
                var result = await _azureBlobService.UploadFileAsync("tutorialapp", "CourseImages", courseDTO.CategoryId + courseDTO.InstructorName + courseDTO.Image.FileName,
                fileStream);
                if (result.IsError)
                {
                    return BadRequest(result);
                }

                courseDTO.CourseImageUrl = result.FileUri.ToString();
            }
            else
            {
                courseDTO.CourseImageUrl = null;
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
