using TutorialApp.Models;
using TutorialApp.Models.DTOs.Enrollment;

namespace TutorialApp.Interfaces
{
    public interface IEnrollmentService
    {
        Task<Enrollment> EnrollCourseAsync(EnrollmentDTO enrollmentDTO);
        Task<IEnumerable<EnrolledCoursesDTO>> GetEnrolledCoursesByUserAsync(string userEmail);
        Task<Enrollment> UpdateEnrollmentStatusAsync(EnrollmentDTO enrollmentDTO);
    }
}
