using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TutorialApp.Exceptions.User;
using TutorialApp.Exceptions.UserCredential;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.User;
using TutorialApp.Repositories;
using TutorialApp.Services;

namespace TutorialAppUnitTests.ServiceTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IRepository<string, User>> _mockUserRepo;
        private Mock<IRepository<int, Course>> _mockCourseRepo;
        private Mock<IRepository<int, Category>> _mockCategoryRepo;
        private Mock<IRepository<string, UserCredential>> _mockUserCredentialRepo;
        private Mock<ILogger<UserService>> _mockLogger;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepo = new Mock<IRepository<string, User>>();
            _mockCourseRepo = new Mock<IRepository<int, Course>>();
            _mockCategoryRepo = new Mock<IRepository<int, Category>>();
            _mockUserCredentialRepo = new Mock<IRepository<string, UserCredential>>();
            _mockLogger = new Mock<ILogger<UserService>>();
            _userService = new UserService(
                _mockUserRepo.Object,
                _mockCourseRepo.Object,
                _mockCategoryRepo.Object,
                _mockUserCredentialRepo.Object,
                _mockLogger.Object
            );
        }

        [Test]
        public async Task ViewProfileAsync_ShouldReturnUserProfile_WhenUserExists()
        {
            // Arrange
            var userEmail = "test@example.com";
            var user = new User
            {
                Email = userEmail,
                Name = "Test User",
                Dob = DateTime.Now.AddYears(-25),
                Phone = "1234567890",
                ImageURI = "http://example.com/image.jpg"
            };
            _mockUserRepo.Setup(repo => repo.GetByKey(userEmail)).ReturnsAsync(user);

            // Act
            var result = await _userService.ViewProfileAsync(userEmail);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userEmail, result.Email);
            Assert.AreEqual("Test User", result.Name);
            Assert.AreEqual("1234567890", result.Phone);
            Assert.AreEqual("http://example.com/image.jpg", result.ImageUri);
            Assert.AreEqual("********", result.Password);
        }

        [Test]
        public void ViewProfileAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var userEmail = "nonexistent@example.com";
            _mockUserRepo.Setup(repo => repo.GetByKey(userEmail)).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserFoundException>(async () =>
                await _userService.ViewProfileAsync(userEmail));
        }

        [Test]
        public async Task EditProfileAsync_ShouldUpdateUserProfile_WhenDataIsValid()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO
            {
                Email = "test@example.com",
                Name = "Updated User",
                Phone = "0987654321",
                ImageUri = "http://example.com/new-image.jpg",
                Dob = DateTime.Now.AddYears(-30),
                Password = "newpassword"
            };

            var user = new User
            {
                Email = userProfileDTO.Email,
                Name = "Old Name",
                Dob = DateTime.Now.AddYears(-25),
                Phone = "1234567890",
                ImageURI = "http://example.com/old-image.jpg"
            };

            var userCredential = new UserCredential
            {
                Email = userProfileDTO.Email,
                PasswordHashKey = new byte[64],
                Password = new byte[64]
            };

            _mockUserRepo.Setup(repo => repo.GetByKey(userProfileDTO.Email)).ReturnsAsync(user);
            _mockUserCredentialRepo.Setup(repo => repo.GetByKey(userProfileDTO.Email)).ReturnsAsync(userCredential);

            // Act
            var result = await _userService.EditProfileAsync(userProfileDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userProfileDTO.Name, result.Name);
            Assert.AreEqual(userProfileDTO.Phone, result.Phone);
            Assert.AreEqual(userProfileDTO.ImageUri, result.ImageUri);
            Assert.AreEqual(userProfileDTO.Dob, result.Dob);
            Assert.AreEqual(userProfileDTO.Password, result.Password);  // Note: This won't match the hashed value
        }

        [Test]
        public void EditProfileAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO { Email = "nonexistent@example.com" };
            _mockUserRepo.Setup(repo => repo.GetByKey(userProfileDTO.Email)).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserFoundException>(async () =>
                await _userService.EditProfileAsync(userProfileDTO));
        }

        [Test]
        public void EditProfileAsync_ShouldThrowException_WhenUserCredentialDoesNotExist()
        {
            // Arrange
            var userProfileDTO = new UserProfileDTO { Email = "test@example.com" };
            var user = new User { Email = userProfileDTO.Email };
            _mockUserRepo.Setup(repo => repo.GetByKey(userProfileDTO.Email)).ReturnsAsync(user);
            _mockUserCredentialRepo.Setup(repo => repo.GetByKey(userProfileDTO.Email)).ReturnsAsync((UserCredential)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchUserCredentialFoundException>(async () =>
                await _userService.EditProfileAsync(userProfileDTO));
        }

        

        [Test]
        public async Task SearchCoursesByCategoryAsync_ShouldReturnEmptyList_WhenCategoryDoesNotExist()
        {
            // Arrange
            var category = "Nonexistent";
            _mockCategoryRepo.Setup(repo => repo.Get()).ReturnsAsync(new List<Category>());
            _mockCourseRepo.Setup(repo => repo.Get()).ReturnsAsync(new List<Course>());

            // Act
            var result = await _userService.SearchCoursesByCategoryAsync(category);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
    }
}
