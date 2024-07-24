using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.User;

namespace TutorialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IAuthService _authService;

        public AdminController(IAdminService adminService, IAuthService authService)
        {
            _adminService = adminService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAdminAsync(UserLoginDTO adminLoginDTO)
        {
            var response = await _authService.LoginAsync(adminLoginDTO);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
