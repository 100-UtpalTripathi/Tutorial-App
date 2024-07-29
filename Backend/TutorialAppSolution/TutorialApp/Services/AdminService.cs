using TutorialApp.Exceptions.Course;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Course;
using TutorialApp.Models.DTOs.User;

namespace TutorialApp.Services
{
    public class AdminService : IAdminService
    {
        #region Dependency Injection
        private readonly IRepository<int, Course> _courseRepository;
        private readonly ILogger<AdminService> _logger;

        public AdminService(IRepository<int, Course> courseRepository, ILogger<AdminService> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        #endregion


        #region Create Course
        public async Task<Course> CreateCourseAsync(CourseDTO courseDTO)
        {
            var course = new Course
            {
                Title = courseDTO.Title,
                Description = courseDTO.Description,
                CategoryId = courseDTO.CategoryId,
                Price = courseDTO.Price,
                CourseImageUrl = courseDTO.CourseImageUrl,
                InstructorName = courseDTO.InstructorName
            };

            try
            {
                var createdCourse = await _courseRepository.Add(course);
                return createdCourse;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new CourseCreationFailedException(ex.Message);
            }
            
        }

        #endregion

        #region Delete Course

        public async Task<Course> DeleteCourseAsync(int courseId)
        {
            try
            {
                var deletedCourse = await _courseRepository.DeleteByKey(courseId);
                if (deletedCourse == null)
                {
                    throw new NoSuchCourseFoundException();
                }
                return deletedCourse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new CourseDeletionFailedException(ex.Message);
            }
            
        }

        #endregion

        #region Get All Courses

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.Get();
        }

        #endregion

        #region Update Course

        public async Task<Course> UpdateCourseAsync(int courseId, CourseDTO courseDTO)
        {
            var existingCourse = await _courseRepository.GetByKey(courseId);
            if (existingCourse == null)
            {
                throw new NoSuchCourseFoundException();
            }

            existingCourse.Title = courseDTO.Title;
            existingCourse.Description = courseDTO.Description;
            existingCourse.CategoryId = courseDTO.CategoryId;
            existingCourse.Price = courseDTO.Price;
            existingCourse.CourseImageUrl = courseDTO.CourseImageUrl;
            existingCourse.InstructorName = courseDTO.InstructorName;

            try
            {
                var updatedCourse = await _courseRepository.Update(existingCourse);
                return updatedCourse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new CourseUpdateFailedException(ex.Message);
            }
            
        }

        #endregion
    }
}
