﻿using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.Enrollment;
using TutorialApp.Models;
using TutorialApp.Exceptions.Enrollment;

namespace TutorialApp.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        #region Dependency Injection
        private readonly IRepository<int, Enrollment> _enrollmentRepository;
        private readonly IRepository<int, Course> _courseRepository;
        private readonly ILogger<EnrollmentService> _logger;

        public EnrollmentService(IRepository<int, Enrollment> enrollmentRepository, IRepository<int, Course> courseRepository, ILogger<EnrollmentService> logger)
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
            _logger = logger;
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

            try
            {
                return await _enrollmentRepository.Add(enrollment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Enrollment failed.");
                throw new EnrollmentFailedException("Enrollment failed.");
            }
            
        }

        #endregion

        #region Get Enrolled Courses By User

        public async Task<IEnumerable<EnrolledCoursesDTO>> GetEnrolledCoursesByUserAsync(string userEmail)
        {
            var enrollments = await _enrollmentRepository.Get();

            if (enrollments == null || enrollments.Count() == 0)
                throw new NoSuchEnrollmentFoundException("No enrollments found.");

            var userEnrollments = enrollments.Where(e => e.UserEmail == userEmail);
            var courses = new List<EnrolledCoursesDTO>();

            foreach (var enrollment in userEnrollments)
            {
                var course = await _courseRepository.GetByKey(enrollment.CourseId);
                if (course != null)
                {
                    var enrolledCourse = new EnrolledCoursesDTO
                    {
                        CourseId = course.CourseId,
                        Title = course.Title,
                        Description = course.Description,
                        Status = enrollment.Status,
                        CategoryName = course.CategoryName,
                        Price = course.Price,
                        CourseImageUrl = course.CourseImageUrl,
                        InstructorName = course.InstructorName
                    };

                    courses.Add(enrolledCourse);
                }
            }

            return courses;
            
        }

        #endregion

        #region Update Enrollment Status

        public async Task<Enrollment> UpdateEnrollmentStatusAsync(EnrollmentDTO enrollmentDTO)
        {
            try
            {
                // Fetch all enrollments
                var enrollments = await _enrollmentRepository.Get();

                // Check if there are any enrollments
                if (enrollments == null || !enrollments.Any())
                {
                    throw new NoSuchEnrollmentFoundException("No enrollments found.");
                }

                // Find the specific enrollment to update
                var enrollment = enrollments.FirstOrDefault(e => e.UserEmail == enrollmentDTO.UserEmail && e.CourseId == enrollmentDTO.CourseId);

                // Check if the enrollment was found
                if (enrollment == null)
                {
                    throw new NoSuchEnrollmentFoundException("Enrollment not found.");
                }

                // Update enrollment status and completion date if necessary
                enrollment.Status = enrollmentDTO.Status;
                if (enrollmentDTO.Status == "Completed")
                {
                    enrollment.CompletionDate = DateTime.Now;
                }

                // Update the enrollment in the repository
                return await _enrollmentRepository.Update(enrollment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Enrollment update failed.");
                throw new EnrollmentFailedException("Enrollment update failed.");
            }
        }

        #endregion

        #region Get All Enrolled Courses
        public async Task<IEnumerable<EnrolledCoursesDTO>> GetAllEnrolledCoursesAsync()
        {
            var enrollments = await _enrollmentRepository.Get();

            if (enrollments == null || enrollments.Count() == 0)
                throw new NoSuchEnrollmentFoundException("No enrollments found.");

            var courses = new List<EnrolledCoursesDTO>();

            foreach (var enrollment in enrollments)
            {
                var course = await _courseRepository.GetByKey(enrollment.CourseId);
                if (course != null)
                {
                    var enrolledCourse = new EnrolledCoursesDTO
                    {
                        CourseId = course.CourseId,
                        Title = course.Title,
                        Description = course.Description,
                        Status = enrollment.Status,
                        CategoryName = course.CategoryName,
                        Price = course.Price,
                        CourseImageUrl = course.CourseImageUrl,
                        InstructorName = course.InstructorName
                    };

                    courses.Add(enrolledCourse);
                }
            }

            return courses;
        }

        #endregion
    }
}
