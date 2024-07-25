using TutorialApp.Models;
using TutorialApp.Models.DTOs.Enrollment;

namespace TutorialApp.Interfaces
{
    public interface IEnrollmentService
    {
        Task<Enrollment> EnrollCourseAsync(EnrollmentDTO enrollmentDTO);
        Task<IEnumerable<Course>> GetEnrolledCoursesByUserAsync(string userEmail);
        Task<Enrollment> UpdateEnrollmentStatusAsync(EnrollmentDTO enrollmentDTO);
    }
}
