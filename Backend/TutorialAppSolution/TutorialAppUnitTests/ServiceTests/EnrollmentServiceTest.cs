using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Exceptions.Enrollment;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Enrollment;
using TutorialApp.Services;

namespace TutorialAppUnitTests.ServiceTests
{
    [TestFixture]
    public class EnrollmentServiceTests
    {
        private Mock<IRepository<int, Enrollment>> _mockEnrollmentRepository;
        private Mock<IRepository<int, Course>> _mockCourseRepository;
        private Mock<ILogger<EnrollmentService>> _mockLogger;
        private EnrollmentService _enrollmentService;

        [SetUp]
        public void Setup()
        {
            _mockEnrollmentRepository = new Mock<IRepository<int, Enrollment>>();
            _mockCourseRepository = new Mock<IRepository<int, Course>>();
            _mockLogger = new Mock<ILogger<EnrollmentService>>();
            _enrollmentService = new EnrollmentService(_mockEnrollmentRepository.Object, _mockCourseRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task EnrollCourseAsync_ShouldCreateAndReturnEnrollment_WhenValidDTOProvided()
        {
            // Arrange
            var enrollmentDTO = new EnrollmentDTO { UserEmail = "user@example.com", CourseId = 1 };
            var enrollment = new Enrollment
            {
                UserEmail = enrollmentDTO.UserEmail,
                CourseId = enrollmentDTO.CourseId,
                Status = "Registered",
                EnrollmentDate = DateTime.Now
            };
            _mockEnrollmentRepository.Setup(repo => repo.Add(It.IsAny<Enrollment>())).ReturnsAsync(enrollment);

            // Act
            var result = await _enrollmentService.EnrollCourseAsync(enrollmentDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(enrollmentDTO.UserEmail, result.UserEmail);
            Assert.AreEqual(enrollmentDTO.CourseId, result.CourseId);
        }

        [Test]
        public void EnrollCourseAsync_ShouldThrowEnrollmentFailedException_WhenExceptionOccurs()
        {
            // Arrange
            var enrollmentDTO = new EnrollmentDTO { UserEmail = "user@example.com", CourseId = 1 };
            _mockEnrollmentRepository.Setup(repo => repo.Add(It.IsAny<Enrollment>())).ThrowsAsync(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<EnrollmentFailedException>(async () => await _enrollmentService.EnrollCourseAsync(enrollmentDTO));
        }

        [Test]
        public async Task GetEnrolledCoursesByUserAsync_ShouldReturnCourses_WhenEnrollmentsExist()
        {
            // Arrange
            var userEmail = "user@example.com";
            var enrollments = new List<Enrollment>
            {
                new Enrollment { UserEmail = userEmail, CourseId = 1 },
                new Enrollment { UserEmail = userEmail, CourseId = 2 }
            };
            var courses = new List<Course>
            {
                new Course { CourseId = 1, Title = "Course 1" },
                new Course { CourseId = 2, Title = "Course 2" }
            };
            _mockEnrollmentRepository.Setup(repo => repo.Get()).ReturnsAsync(enrollments);
            _mockCourseRepository.Setup(repo => repo.GetByKey(It.IsAny<int>())).ReturnsAsync((int id) => courses.FirstOrDefault(c => c.CourseId == id));

            // Act
            var result = await _enrollmentService.GetEnrolledCoursesByUserAsync(userEmail);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(c => c.Title == "Course 1"));
            Assert.IsTrue(result.Any(c => c.Title == "Course 2"));
        }

        [Test]
        public void GetEnrolledCoursesByUserAsync_ShouldThrowNoSuchEnrollmentFoundException_WhenNoEnrollmentsExist()
        {
            // Arrange
            var userEmail = "user@example.com";
            _mockEnrollmentRepository.Setup(repo => repo.Get()).ReturnsAsync(new List<Enrollment>());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchEnrollmentFoundException>(async () => await _enrollmentService.GetEnrolledCoursesByUserAsync(userEmail));
        }

        [Test]
        public async Task UpdateEnrollmentStatusAsync_ShouldUpdateAndReturnEnrollment_WhenValidDTOProvided()
        {
            // Arrange
            var enrollmentDTO = new EnrollmentDTO { UserEmail = "user@example.com", CourseId = 1, Status = "Completed" };
            var enrollment = new Enrollment
            {
                UserEmail = enrollmentDTO.UserEmail,
                CourseId = enrollmentDTO.CourseId,
                Status = "Registered"
            };
            _mockEnrollmentRepository.Setup(repo => repo.Get()).ReturnsAsync(new List<Enrollment> { enrollment });
            _mockEnrollmentRepository.Setup(repo => repo.Update(It.IsAny<Enrollment>())).ReturnsAsync(enrollment);

            // Act
            var result = await _enrollmentService.UpdateEnrollmentStatusAsync(enrollmentDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(enrollmentDTO.Status, result.Status);
            Assert.IsNotNull(result.CompletionDate);
        }

        [Test]
        public void UpdateEnrollmentStatusAsync_ShouldThrowEnrollmentFailedException_WhenEnrollmentNotFound()
        {
            // Arrange
            var enrollmentDTO = new EnrollmentDTO { UserEmail = "user@example.com", CourseId = 1, Status = "Completed" };
            _mockEnrollmentRepository.Setup(repo => repo.Get()).ReturnsAsync(new List<Enrollment>());

            // Act & Assert
            Assert.ThrowsAsync<EnrollmentFailedException>(async () => await _enrollmentService.UpdateEnrollmentStatusAsync(enrollmentDTO));
        }

        [Test]
        public void UpdateEnrollmentStatusAsync_ShouldThrowEnrollmentFailedException_WhenExceptionOccurs()
        {
            // Arrange
            var enrollmentDTO = new EnrollmentDTO { UserEmail = "user@example.com", CourseId = 1, Status = "Completed" };
            var enrollment = new Enrollment
            {
                UserEmail = enrollmentDTO.UserEmail,
                CourseId = enrollmentDTO.CourseId,
                Status = "Registered"
            };
            _mockEnrollmentRepository.Setup(repo => repo.Get()).ReturnsAsync(new List<Enrollment> { enrollment });
            _mockEnrollmentRepository.Setup(repo => repo.Update(It.IsAny<Enrollment>())).ThrowsAsync(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<EnrollmentFailedException>(async () => await _enrollmentService.UpdateEnrollmentStatusAsync(enrollmentDTO));
        }
    }
}
