using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.User;

namespace TutorialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAzureBlobService _azureBlobService;
        public AuthController(IAuthService authService, IAzureBlobService azureBlobService)
        {
            _authService = authService;
            _azureBlobService = azureBlobService;
        }

        [HttpPost("user/register")]
        public async Task<IActionResult> RegisterAsync(UserRegisterDTO userRegisterDTO)
        {
            if(userRegisterDTO.Image != null)
            {
                var fileStream = userRegisterDTO.Image.OpenReadStream();
                var result = await _azureBlobService.UploadFileAsync("tutorialapp", "user", userRegisterDTO.Image.FileName,
                    fileStream);

                if (result.IsError)
                {
                    return BadRequest(result);
                }

                userRegisterDTO.ImageUri = result.FileUri.ToString();
            }
            else
            {
                userRegisterDTO.ImageUri = null;
            }
            

            var response = await _authService.RegisterAsync(userRegisterDTO);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("user/login")]
        public async Task<IActionResult> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var response = await _authService.LoginAsync(userLoginDTO);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }   
    }
}
