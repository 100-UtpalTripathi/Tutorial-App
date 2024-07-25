using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.Enrollment;
using TutorialApp.Models;

namespace TutorialApp.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        #region Dependency Injection
        private readonly IRepository<int, Enrollment> _enrollmentRepository;
        private readonly IRepository<int, Course> _courseRepository;

        public EnrollmentService(IRepository<int, Enrollment> enrollmentRepository, IRepository<int, Course> courseRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
        }

        #endregion


        #region Enroll Course
        public async Task<Enrollment> EnrollCourseAsync(EnrollmentDTO enrollmentDTO)
        {
            var enrollment = new Enrollment
            {
                UserEmail = enrollmentDTO.UserEmail,
                CourseId = enrollmentDTO.CourseId,
                Status = "Registered",
                EnrollmentDate = DateTime.Now
            };
            return await _enrollmentRepository.Add(enrollment);
        }

        #endregion

        #region Get Enrolled Courses By User

        public async Task<IEnumerable<Course>> GetEnrolledCoursesByUserAsync(string userEmail)
        {
            var enrollments = await _enrollmentRepository.Get();
            var userEnrollments = enrollments.Where(e => e.UserEmail == userEmail);
            var courses = new List<Course>();

            foreach (var enrollment in userEnrollments)
            {
                var course = await _courseRepository.GetByKey(enrollment.CourseId);
                if (course != null)
                {
                    courses.Add(course);
                }
            }

            return courses;
        }

        #endregion

        #region Update Enrollment Status

        public async Task<Enrollment> UpdateEnrollmentStatusAsync(EnrollmentDTO enrollmentDTO)
        {
            var enrollments = await _enrollmentRepository.Get();
            var enrollment = enrollments.FirstOrDefault(e => e.UserEmail == enrollmentDTO.UserEmail && e.CourseId == enrollmentDTO.CourseId);
            if (enrollment != null)
            {
                enrollment.Status = enrollmentDTO.Status;
                if (enrollmentDTO.Status == "Completed")
                {
                    enrollment.CompletionDate = DateTime.Now;
                }
                return await _enrollmentRepository.Update(enrollment);
            }
            throw new Exception("Enrollment not found.");
        }

        #endregion
    }
}
