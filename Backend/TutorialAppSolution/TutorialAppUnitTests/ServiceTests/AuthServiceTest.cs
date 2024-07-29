using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TutorialApp.Exceptions.User;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.User;
using TutorialApp.Services;

namespace TutorialAppUnitTests.ServiceTests
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IRepository<string, User>> _mockUserRepo;
        private Mock<IRepository<string, UserCredential>> _mockUserCredentialRepo;
        private Mock<ITokenService> _mockTokenService;
        private Mock<ILogger<AuthService>> _mockLogger;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepo = new Mock<IRepository<string, User>>();
            _mockUserCredentialRepo = new Mock<IRepository<string, UserCredential>>();
            _mockTokenService = new Mock<ITokenService>();
            _mockLogger = new Mock<ILogger<AuthService>>();
            _authService = new AuthService(_mockUserRepo.Object, _mockUserCredentialRepo.Object, _mockTokenService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var userLoginDTO = new UserLoginDTO
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var passwordHashKey = new HMACSHA512().Key;
            var hashedPassword = new HMACSHA512(passwordHashKey).ComputeHash(Encoding.UTF8.GetBytes(userLoginDTO.Password));

            var userCredential = new UserCredential
            {
                Email = userLoginDTO.Email,
                PasswordHashKey = passwordHashKey,
                Password = hashedPassword,
                Status = "Active"
            };

            var user = new User
            {
                Email = userLoginDTO.Email,
                Role = "User"
            };

            _mockUserCredentialRepo.Setup(repo => repo.GetByKey(userLoginDTO.Email)).ReturnsAsync(userCredential);
            _mockUserRepo.Setup(repo => repo.GetByKey(userLoginDTO.Email)).ReturnsAsync(user);
            _mockTokenService.Setup(ts => ts.GenerateToken(user)).Returns("token123");

            // Act
            var result = await _authService.LoginAsync(userLoginDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userLoginDTO.Email, result.Email);
            Assert.AreEqual(user.Role, result.Role);
            Assert.AreEqual("token123", result.Token);
        }

        [Test]
        public void LoginAsync_ShouldThrowUnauthorizedUserException_WhenInvalidPassword()
        {
            // Arrange
            var userLoginDTO = new UserLoginDTO
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var passwordHashKey = new HMACSHA512().Key;
            var hashedPassword = new HMACSHA512(passwordHashKey).ComputeHash(Encoding.UTF8.GetBytes("password123"));

            var userCredential = new UserCredential
            {
                Email = userLoginDTO.Email,
                PasswordHashKey = passwordHashKey,
                Password = hashedPassword,
                Status = "Active"
            };

            _mockUserCredentialRepo.Setup(repo => repo.GetByKey(userLoginDTO.Email)).ReturnsAsync(userCredential);

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedUserException>(async () => await _authService.LoginAsync(userLoginDTO));
        }

        [Test]
        public void LoginAsync_ShouldThrowUnauthorizedUserException_WhenUserIsInactive()
        {
            // Arrange
            var userLoginDTO = new UserLoginDTO
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var passwordHashKey = new HMACSHA512().Key;
            var hashedPassword = new HMACSHA512(passwordHashKey).ComputeHash(Encoding.UTF8.GetBytes(userLoginDTO.Password));

            var userCredential = new UserCredential
            {
                Email = userLoginDTO.Email,
                PasswordHashKey = passwordHashKey,
                Password = hashedPassword,
                Status = "Inactive"
            };

            _mockUserCredentialRepo.Setup(repo => repo.GetByKey(userLoginDTO.Email)).ReturnsAsync(userCredential);

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedUserException>(async () => await _authService.LoginAsync(userLoginDTO));
        }

        [Test]
        public async Task RegisterAsync_ShouldRegisterUser_WhenValidDTOProvided()
        {
            // Arrange
            var userRegisterDTO = new UserRegisterDTO
            {
                Email = "newuser@example.com",
                Password = "password123",
                Role = "User",
                Name = "New User",
                Dob = DateTime.Now,
                Phone = "1234567890",
                ImageUri = "http://example.com/image.jpg"
            };

            var passwordHashKey = new HMACSHA512().Key;
            var hashedPassword = new HMACSHA512(passwordHashKey).ComputeHash(Encoding.UTF8.GetBytes(userRegisterDTO.Password));

            var user = new User
            {
                Email = userRegisterDTO.Email,
                Role = userRegisterDTO.Role,
                Name = userRegisterDTO.Name,
                Dob = userRegisterDTO.Dob,
                Phone = userRegisterDTO.Phone,
                ImageURI = userRegisterDTO.ImageUri
            };

            var userCredential = new UserCredential
            {
                Email = userRegisterDTO.Email,
                PasswordHashKey = passwordHashKey,
                Password = hashedPassword,
                Status = "Active"
            };

            _mockUserRepo.Setup(repo => repo.Add(It.IsAny<User>())).ReturnsAsync(user);
            _mockUserCredentialRepo.Setup(repo => repo.Add(It.IsAny<UserCredential>())).ReturnsAsync(userCredential);

            // Act
            var result = await _authService.RegisterAsync(userRegisterDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userRegisterDTO.Email, result.Email);
            Assert.AreEqual(userRegisterDTO.Role, result.Role);
        }

        [Test]
        public void RegisterAsync_ShouldThrowUserRegistrationFailedException_WhenExceptionOccurs()
        {
            // Arrange
            var userRegisterDTO = new UserRegisterDTO
            {
                Email = "newuser@example.com",
                Password = "password123"
            };

            _mockUserRepo.Setup(repo => repo.Add(It.IsAny<User>())).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            Assert.ThrowsAsync<UserRegistrationFailedException>(async () => await _authService.RegisterAsync(userRegisterDTO));
        }

        
        [Test]
        public void ComparePassword_ShouldReturnTrue_WhenPasswordsMatch()
        {
            // Arrange
            var password = "password123";
            var hmac = new HMACSHA512();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashedPassword = hmac.ComputeHash(passwordBytes);
            var inputPasswordHash = hmac.ComputeHash(passwordBytes); // Hashing the input password to compare with

            // Act
            var result = _authService.ComparePassword(hashedPassword, inputPasswordHash);

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public void ComparePassword_ShouldReturnFalse_WhenPasswordsDoNotMatch()
        {
            // Arrange
            var password = "password123";
            var differentPassword = "differentpassword";
            var hmac = new HMACSHA512();
            var hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var inputPassword = Encoding.UTF8.GetBytes(differentPassword);

            // Act
            var result = _authService.ComparePassword(hashedPassword, inputPassword);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
