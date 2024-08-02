using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TutorialApp.Exceptions.Course;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Course;
using TutorialApp.Services;

namespace TutorialAppUnitTests.ServiceTests
{
    [TestFixture]
    public class AdminServiceTests
    {
        private Mock<IRepository<int, Course>> _mockCourseRepository;
        private Mock<IRepository<int, Category>> _mockCategoryRepository;
        private Mock<ILogger<AdminService>> _mockLogger;
        private AdminService _adminService;

        [SetUp]
        public void Setup()
        {
            _mockCourseRepository = new Mock<IRepository<int, Course>>();
            _mockCategoryRepository = new Mock<IRepository<int, Category>>();

            _mockLogger = new Mock<ILogger<AdminService>>();
            _adminService = new AdminService(_mockCourseRepository.Object, _mockLogger.Object, _mockCategoryRepository.Object);
        }

        

        [Test]
        public async Task DeleteCourseAsync_ShouldDeleteCourse_WhenCourseExists()
        {
            // Arrange
            var courseId = 1;
            var course = new Course { CourseId = courseId, Title = "Course Title" };

            _mockCourseRepository.Setup(repo => repo.DeleteByKey(courseId)).ReturnsAsync(course);

            // Act
            var result = await _adminService.DeleteCourseAsync(courseId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(courseId, result.CourseId);
        }

        [Test]
        public void DeleteCourseAsync_ShouldThrowNoSuchCourseFoundException_WhenCourseNotFound()
        {
            // Arrange
            var courseId = 1;
            _mockCourseRepository.Setup(repo => repo.DeleteByKey(courseId)).ReturnsAsync((Course)null);

            // Act & Assert
            Assert.ThrowsAsync<CourseDeletionFailedException>(async () => await _adminService.DeleteCourseAsync(courseId));
        }

        [Test]
        public void DeleteCourseAsync_ShouldThrowCourseDeletionFailedException_WhenExceptionOccurs()
        {
            // Arrange
            var courseId = 1;
            _mockCourseRepository.Setup(repo => repo.DeleteByKey(courseId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            Assert.ThrowsAsync<CourseDeletionFailedException>(async () => await _adminService.DeleteCourseAsync(courseId));
        }

        [Test]
        public async Task GetAllCoursesAsync_ShouldReturnCourses_WhenCoursesExist()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course { CourseId = 1, Title = "Course 1" },
                new Course { CourseId = 2, Title = "Course 2" }
            };

            _mockCourseRepository.Setup(repo => repo.Get()).ReturnsAsync(courses);

            // Act
            var result = await _adminService.GetAllCoursesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllCoursesAsync_ShouldReturnEmptyList_WhenNoCoursesExist()
        {
            // Arrange
            _mockCourseRepository.Setup(repo => repo.Get()).ReturnsAsync(new List<Course>());

            // Act
            var result = await _adminService.GetAllCoursesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        

        [Test]
        public void UpdateCourseAsync_ShouldThrowNoSuchCourseFoundException_WhenCourseNotFound()
        {
            // Arrange
            var courseId = 1;
            var courseDTO = new CourseDTO
            {
                Title = "Updated Title",
                Description = "Updated Description",
                CategoryName = "Web Development",
                Price = 150m,
                CourseImageUrl = "http://example.com/newimage.jpg",
                InstructorName = "New Instructor"
            };

            _mockCourseRepository.Setup(repo => repo.GetByKey(courseId)).ReturnsAsync((Course)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchCourseFoundException>(async () => await _adminService.UpdateCourseAsync(courseId, courseDTO));
        }

        
    }
}
