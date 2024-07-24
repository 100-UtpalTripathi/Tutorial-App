using TutorialApp.Models;

namespace TutorialApp.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
