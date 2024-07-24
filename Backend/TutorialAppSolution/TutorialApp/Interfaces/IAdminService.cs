using TutorialApp.Models;
using TutorialApp.Models.DTOs.Course;
using TutorialApp.Models.DTOs.User;

namespace TutorialApp.Interfaces
{
    public interface IAdminService
    {
        Task<CourseReturnDTO> CreateCourseAsync(CourseDTO courseDTO);
        Task<CourseReturnDTO> UpdateCourseAsync(CourseDTO courseDTO);
        Task<CourseReturnDTO> DeleteCourseAsync(int courseId);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
    }
}
