﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.User;
using TutorialApp.Models.DTOs;
using System.Net;
using TutorialApp.Exceptions.User;
using Microsoft.AspNetCore.Cors;
using TutorialApp.Models.DTOs.Course;

namespace TutorialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class AuthController : ControllerBase
    {

        #region Dependency Injection
        private readonly IAuthService _authService;
        private readonly IAzureBlobService _azureBlobService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IAzureBlobService azureBlobService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _azureBlobService = azureBlobService;
            _logger = logger;
        }

        #endregion

        #region Register

        [HttpPost("user/register")]
        public async Task<IActionResult> RegisterAsync([FromForm] UserRegisterDTO userRegisterDTO)
        {
            if (userRegisterDTO.Image != null)
            {
                var fileStream = userRegisterDTO.Image.OpenReadStream();
                // Generate a unique file name
                var uniqueFileName = GenerateUniqueFileName(userRegisterDTO.Image.FileName);

                // Upload file with a unique name
                var result = await _azureBlobService.UploadFileAsync("tutorialapp", "CourseImages", uniqueFileName, fileStream);

                if (result.IsError)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Image upload failed", null);
                    return BadRequest(errorResponse);
                }

                userRegisterDTO.ImageUri = result.FileUri.ToString();
            }
            else
            {
                userRegisterDTO.ImageUri = null;
            }

            try
            {
                var response = await _authService.RegisterAsync(userRegisterDTO);
                if (response == null)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Registration failed", null);
                    return BadRequest(errorResponse);
                }
                var successResponse = new ApiResponse<UserRegisterReturnDTO>((int)HttpStatusCode.OK, "Registration successful", response);
                return Ok(successResponse);
            }
            catch(UserRegistrationFailedException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null));
            }

            
        }

        #endregion

        #region Login

        [HttpPost("user/login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {
                var response = await _authService.LoginAsync(userLoginDTO);
                if (response == null)
                {
                    return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Invalid login details", null));
                }
                return Ok(new ApiResponse<UserLoginReturnDTO>((int)HttpStatusCode.OK, "Login successful", response));
            }
            catch (NoSuchUserFoundException)
            {
                _logger.LogError("User not found");
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null));
            }
            catch (UnauthorizedUserException)
            {
                _logger.LogError("Invalid Email or Password");
                return Unauthorized(new ApiResponse<string>((int)HttpStatusCode.Unauthorized, "Invalid Email or Password", null));
            }
            catch (Exception ex)
            {_logger.LogError(ex.Message);
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null));
            }

        }
        private string GenerateUniqueFileName(string originalFileName)
        {
            var fileExtension = Path.GetExtension(originalFileName);
            var uniqueFileName = $"{Guid.NewGuid()}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";
            return uniqueFileName;
        }

        #endregion
    }
}
