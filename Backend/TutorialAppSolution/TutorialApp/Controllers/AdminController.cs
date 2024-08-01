using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Exceptions.User;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.User;
using System.Net;
using Microsoft.AspNetCore.Cors;

namespace TutorialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]    
   
    public class AdminController : ControllerBase
    {
        #region Dependency Injection
        private readonly IAdminService _adminService;
        private readonly IAuthService _authService;
        private readonly ILogger<AdminController> _logger;
        public AdminController(IAdminService adminService, IAuthService authService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _authService = authService;
            _logger = logger;
        }
        #endregion


        #region Login

        [HttpPost("login")]
        public async Task<IActionResult> LoginAdminAsync([FromBody] UserLoginDTO adminLoginDTO)
        {
            try
            {
                var response = await _authService.LoginAsync(adminLoginDTO);
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
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null));
            }
        }

        #endregion
    }
}
