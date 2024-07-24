using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Course;
using TutorialApp.Models.DTOs.User;

namespace TutorialApp.Services
{
    public class AdminService : IAdminService
    {
        public Task<CourseReturnDTO> CreateCourseAsync(CourseDTO courseDTO)
        {
            throw new NotImplementedException();
        }

        public Task<CourseReturnDTO> DeleteCourseAsync(int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CourseReturnDTO> UpdateCourseAsync(CourseDTO courseDTO)
        {
            throw new NotImplementedException();
        }
    }
}
