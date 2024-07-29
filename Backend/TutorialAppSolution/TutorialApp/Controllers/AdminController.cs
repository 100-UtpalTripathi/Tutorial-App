using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Exceptions.User;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.User;
using System.Net;

namespace TutorialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

   
    public class AdminController : ControllerBase
    {
        #region Dependency Injection
        private readonly IAdminService _adminService;
        private readonly IAuthService _authService;

        public AdminController(IAdminService adminService, IAuthService authService)
        {
            _adminService = adminService;
            _authService = authService;
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
                return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, "User not found", null));
            }
            catch (UnauthorizedUserException)
            {
                return Unauthorized(new ApiResponse<string>((int)HttpStatusCode.Unauthorized, "Invalid Email or Password", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null));
            }
        }

        #endregion
    }
}
