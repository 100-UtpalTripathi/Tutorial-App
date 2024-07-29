using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Services;

namespace TutorialAppUnitTests.ServiceTests
{
    public class TokenServiceTest
    {
        private Mock<IConfiguration> _configurationMock;

        [SetUp]
        public void Setup()
        {
            var secretKey = "This is the dummy key which has to be a bit long for the 512. which should be even more longer for the passing";

            var configurationJWTSectionMock = new Mock<IConfigurationSection>();
            configurationJWTSectionMock.Setup(x => x.Value).Returns(secretKey);

            var configurationTokenKeySectionMock = new Mock<IConfigurationSection>();
            configurationTokenKeySectionMock.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSectionMock.Object);

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x.GetSection("TokenKey")).Returns(configurationTokenKeySectionMock.Object);
        }

        [Test]
        public void CreateTokenPassTest()
        {
            // Arrange
            ITokenService service = new TokenService(_configurationMock.Object);

            // Act
            var token = service.GenerateToken(new User { Email = "test@example.com", Role = "Admin" });

            // Assert
            Assert.IsNotNull(token);
        }

        [Test]
        public void TokenContainsCorrectClaimsTest()
        {
            // Arrange
            ITokenService service = new TokenService(_configurationMock.Object);

            // Act
            var token = service.GenerateToken(new User { Email = "test@example.com", Role = "Admin" });
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.AreEqual("test@example.com", jwtToken.Claims.FirstOrDefault(c => c.Type == "uEmailId")?.Value);
            Assert.AreEqual("Admin", jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
        }

        [Test]
        public void TokenHasCorrectExpirationTest()
        {
            // Arrange
            ITokenService service = new TokenService(_configurationMock.Object);

            // Act
            var token = service.GenerateToken(new User { Email = "test@example.com", Role = "Admin" });
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.IsTrue(jwtToken.ValidTo >= DateTime.UtcNow.AddDays(2).AddMinutes(-1) &&
                          jwtToken.ValidTo <= DateTime.UtcNow.AddDays(2).AddMinutes(1));
        }

        [Test]
        public void GenerateTokenInvalidConfigurationTest()
        {
            // Arrange
            var invalidConfigurationMock = new Mock<IConfiguration>();
            var emptyJWTSectionMock = new Mock<IConfigurationSection>();
            emptyJWTSectionMock.Setup(x => x.Value).Returns((string)null); // Simulate missing JWT value
            invalidConfigurationMock.Setup(x => x.GetSection("TokenKey")).Returns(new Mock<IConfigurationSection>().Object);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new TokenService(invalidConfigurationMock.Object));
            Assert.AreEqual("JWT secret key is missing or empty.", ex.Message);
        }

        [Test]
        public void CreateTokenForDifferentRoleTest()
        {
            // Arrange
            ITokenService service = new TokenService(_configurationMock.Object);

            // Act
            var token = service.GenerateToken(new User { Email = "another@example.com", Role = "User" });
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.AreEqual("another@example.com", jwtToken.Claims.FirstOrDefault(c => c.Type == "uEmailId")?.Value);
            Assert.AreEqual("User", jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
        }
    }
}
