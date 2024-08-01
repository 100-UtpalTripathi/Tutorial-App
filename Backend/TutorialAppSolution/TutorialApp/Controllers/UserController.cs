using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs;
using TutorialApp.Models.DTOs.User;
using TutorialApp.Models;
using TutorialApp.Services;
using TutorialApp.Exceptions.User;
using TutorialApp.Exceptions.UserCredential;
using System.Net;
using Microsoft.AspNetCore.Cors;

namespace TutorialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class UserController : ControllerBase
    {
        #region Dependency Injection
        private readonly IUserService _userService;
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IAzureBlobService azureBlobService, ILogger<UserController> logger)
        {
            _userService = userService;
            _azureBlobService = azureBlobService;
            _logger = logger;
        }

        #endregion

        #region View Profile

        [HttpGet("profile/{userEmail}")]
        public async Task<IActionResult> ViewProfile(string userEmail)
        {
            try
            {
                var profile = await _userService.ViewProfileAsync(userEmail);
                if (profile == null)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, "User profile not found", null);
                    return NotFound(errorResponse);
                }
                var response = new ApiResponse<UserProfileDTO>((int)HttpStatusCode.OK, "User profile retrieved successfully", profile);
                return Ok(response);
            }
            catch (NoSuchUserFoundException ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, ex.Message, null);
                return NotFound(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null);
                return BadRequest(errorResponse);
            }
        }

        #endregion

        #region Edit Profile

        [HttpPut("profile/update")]
        public async Task<IActionResult> EditProfile([FromBody] UserProfileDTO userProfileDTO)
        {
            if (userProfileDTO.Image != null)
            {
                var fileStream = userProfileDTO.Image.OpenReadStream();
                var result = await _azureBlobService.UploadFileAsync("tutorialapp", "userProfileUpdated", userProfileDTO.Phone + userProfileDTO.Image.FileName,
                    fileStream);

                if (result.IsError)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Image Update Error!", null);
                    return BadRequest(errorResponse);
                }

                userProfileDTO.ImageUri = result.FileUri.ToString();
            }
            else
            {
                userProfileDTO.ImageUri = null;
            }

            try
            {
                var updatedProfile = await _userService.EditProfileAsync(userProfileDTO);
                if (updatedProfile == null)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, "User profile not found", null);
                    return NotFound(errorResponse);
                }
                var response = new ApiResponse<UserProfileDTO>((int)HttpStatusCode.OK, "User profile updated successfully", updatedProfile);
                return Ok(response);
            }
            catch (NoSuchUserFoundException ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, ex.Message, null);
                return NotFound(errorResponse);
            }
            catch (NoSuchUserCredentialFoundException ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, ex.Message, null);
                return NotFound(errorResponse);
            }
            catch (UserProfileUpdateFailedException ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null);
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null);
                return BadRequest(errorResponse);
            }
        }

        #endregion

        #region Search Courses By Category

        [HttpGet("courses/get/{category}")]
        public async Task<IActionResult> SearchCoursesByCategory(string category)
        {
            try
            {
                var courses = await _userService.SearchCoursesByCategoryAsync(category);
                var response = new ApiResponse<IEnumerable<Course>>((int)HttpStatusCode.OK, "Courses retrieved successfully", courses);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null);
                return BadRequest(errorResponse);
            }
        }

        #endregion
    }
}
