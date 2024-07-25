using TutorialApp.Models;
using TutorialApp.Models.DTOs.Course;
using TutorialApp.Models.DTOs.User;

namespace TutorialApp.Interfaces
{
    public interface IAdminService
    {
        Task<Course> CreateCourseAsync(CourseDTO courseDTO);
        Task<Course> UpdateCourseAsync(int courseId, CourseDTO courseDTO);
        Task<Course> DeleteCourseAsync(int courseId);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
    }
}
