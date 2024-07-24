using TutorialApp.Models.DTOs.User;

namespace TutorialApp.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginReturnDTO> LoginAsync(UserLoginDTO userLoginDTO);
        Task<UserRegisterReturnDTO> RegisterAsync(UserRegisterDTO userRegisterDTO);
    }
}
