using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.User;
using TutorialApp.Models;
using TutorialApp.Services;

namespace TutorialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAzureBlobService _azureBlobService;

        public UserController(IUserService userService, IAzureBlobService azureBlobService)
        {
            _userService = userService;
            _azureBlobService = azureBlobService;
        }

        [HttpGet("profile/{userEmail}")]
        public async Task<ActionResult<UserProfileDTO>> ViewProfile(string userEmail)
        {
            var profile = await _userService.ViewProfileAsync(userEmail);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }

        [HttpPut("profile/update")]
        public async Task<ActionResult<UserProfileDTO>> EditProfile([FromBody] UserProfileDTO userProfileDTO)
        {
            if (userProfileDTO.Image != null)
            {
                var fileStream = userProfileDTO.Image.OpenReadStream();
                var result = await _azureBlobService.UploadFileAsync("tutorialapp", "userProfileUpdated", userProfileDTO.Phone + userProfileDTO.Image.FileName,
                    fileStream);

                if (result.IsError)
                {
                    return BadRequest(result);
                }

                userProfileDTO.ImageUri = result.FileUri.ToString();
            }
            else
            {
                userProfileDTO.ImageUri = null;
            }

            var updatedProfile = await _userService.EditProfileAsync(userProfileDTO);
            if (updatedProfile == null)
            {
                return NotFound();
            }
            return Ok(updatedProfile);
        }

        [HttpGet("courses/get/{category}")]
        public async Task<ActionResult<IEnumerable<Course>>> SearchCoursesByCategory(string category)
        {
            var courses = await _userService.SearchCoursesByCategoryAsync(category);
            return Ok(courses);
        }
    }
}
