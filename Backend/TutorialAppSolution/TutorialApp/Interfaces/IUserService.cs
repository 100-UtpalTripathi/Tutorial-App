using TutorialApp.Models.DTOs.User;
using TutorialApp.Models;

namespace TutorialApp.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDTO> ViewProfileAsync(string userEmail);
        Task<UserProfileDTO> EditProfileAsync(UserProfileDTO userProfileDTO);
        Task<IEnumerable<Course>> SearchCoursesByCategoryAsync(string category);
    }
}
