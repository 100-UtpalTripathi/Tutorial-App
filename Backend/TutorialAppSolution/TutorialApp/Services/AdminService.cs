using TutorialApp.Exceptions.Course;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Course;
using TutorialApp.Models.DTOs.User;

namespace TutorialApp.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<int, Course> _courseRepository;

        public AdminService(IRepository<int, Course> courseRepository)
        {
            _courseRepository = courseRepository;
        }

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

            var createdCourse = await _courseRepository.Add(course);
            return createdCourse;
        }

        public async Task<Course> DeleteCourseAsync(int courseId)
        {
            var deletedCourse = await _courseRepository.DeleteByKey(courseId);
            if (deletedCourse == null)
            {
                throw new NoSuchCourseFoundException();
            }
            return deletedCourse;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.Get();
        }

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

            var updatedCourse = await _courseRepository.Update(existingCourse);
            return updatedCourse;
        }
    }
}
