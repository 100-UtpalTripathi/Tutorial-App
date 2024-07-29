using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.UserCredential;
using TutorialApp.Models;
using TutorialApp.Repositories;

namespace TutorialAppUnitTests.RepositoryTests
{
    public class UserCredentialRepositoryTest
    {
        private DbContextOptions<TutorialAppContext> _options;

        [SetUp]
        public async Task Setup()
        {
            _options = new DbContextOptionsBuilder<TutorialAppContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new TutorialAppContext(_options))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
            }
        }

        [Test]
        public async Task AddUserCredentialAsync_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserCredentialRepository(context);
                var user = new User
                {
                    Email = "testuser@example.com",
                    Name = "Test User",
                    Dob = new DateTime(1990, 1, 1),
                    Phone = "1234567890",
                    Role = "User",
                    ImageURI = "https://example.com/image.jpg"
                };
                var userCredential = new UserCredential
                {
                    Email = "testuser@example.com",
                    PasswordHashKey = new byte[] { 0x01, 0x02 },
                    Password = new byte[] { 0x03, 0x04 },
                    Status = "Active"
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                // Act
                var addedUserCredential = await repository.Add(userCredential);

                // Assert
                Assert.IsNotNull(addedUserCredential);
                Assert.AreEqual(userCredential.Email, addedUserCredential.Email);
                Assert.AreEqual(userCredential.Status, addedUserCredential.Status);
            }
        }

        [Test]
        public async Task DeleteUserCredentialByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserCredentialRepository(context);
                var user = new User
                {
                    Email = "testuser@example.com",
                    Name = "Test User",
                    Dob = new DateTime(1990, 1, 1),
                    Phone = "1234567890",
                    Role = "User",
                    ImageURI = "https://example.com/image.jpg"
                };
                var userCredential = new UserCredential
                {
                    Email = "testuser@example.com",
                    PasswordHashKey = new byte[] { 0x01, 0x02 },
                    Password = new byte[] { 0x03, 0x04 },
                    Status = "Active"
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                await repository.Add(userCredential);

                // Act
                var deletedUserCredential = await repository.DeleteByKey(userCredential.Email);

                // Assert
                Assert.IsNotNull(deletedUserCredential);
                Assert.AreEqual(userCredential.Email, deletedUserCredential.Email);
            }
        }

        [Test]
        public void DeleteUserCredentialByKey_UserCredentialNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserCredentialRepository(context);

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchUserCredentialFoundException>(() => repository.DeleteByKey("nonexistent@example.com"));
                Assert.AreEqual("No such user credential found", ex.Message);
            }
        }

        [Test]
        public async Task GetUserCredentialByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserCredentialRepository(context);
                var user = new User
                {
                    Email = "testuser@example.com",
                    Name = "Test User",
                    Dob = new DateTime(1990, 1, 1),
                    Phone = "1234567890",
                    Role = "User",
                    ImageURI = "https://example.com/image.jpg"
                };
                var userCredential = new UserCredential
                {
                    Email = "testuser@example.com",
                    PasswordHashKey = new byte[] { 0x01, 0x02 },
                    Password = new byte[] { 0x03, 0x04 },
                    Status = "Active"
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                await repository.Add(userCredential);

                // Act
                var fetchedUserCredential = await repository.GetByKey(userCredential.Email);

                // Assert
                Assert.IsNotNull(fetchedUserCredential);
                Assert.AreEqual(userCredential.Email, fetchedUserCredential.Email);
            }
        }

        [Test]
        public async Task GetUserCredentialByKey_UserCredentialNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserCredentialRepository(context);

                // Act
                var userCredential = await repository.GetByKey("nonexistent@example.com");

                // Assert
                Assert.IsNull(userCredential);
            }
        }

        [Test]
        public async Task GetUserCredentials_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserCredentialRepository(context);
                var user = new User
                {
                    Email = "testuser1@example.com",
                    Name = "Test User 1",
                    Dob = new DateTime(1990, 1, 1),
                    Phone = "1234567890",
                    Role = "User",
                    ImageURI = "https://example.com/image1.jpg"
                };
                var userCredential1 = new UserCredential
                {
                    Email = "testuser1@example.com",
                    PasswordHashKey = new byte[] { 0x01, 0x02 },
                    Password = new byte[] { 0x03, 0x04 },
                    Status = "Active"
                };
                var userCredential2 = new UserCredential
                {
                    Email = "testuser2@example.com",
                    PasswordHashKey = new byte[] { 0x05, 0x06 },
                    Password = new byte[] { 0x07, 0x08 },
                    Status = "Inactive"
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                await repository.Add(userCredential1);
                await repository.Add(userCredential2);

                // Act
                var userCredentials = await repository.Get();

                // Assert
                Assert.IsNotNull(userCredentials);
                Assert.AreEqual(2, userCredentials.Count());
            }
        }

        [Test]
        public async Task UpdateUserCredential_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserCredentialRepository(context);
                var user = new User
                {
                    Email = "testuser@example.com",
                    Name = "Test User",
                    Dob = new DateTime(1990, 1, 1),
                    Phone = "1234567890",
                    Role = "User",
                    ImageURI = "https://example.com/image.jpg"
                };
                var userCredential = new UserCredential
                {
                    Email = "testuser@example.com",
                    PasswordHashKey = new byte[] { 0x01, 0x02 },
                    Password = new byte[] { 0x03, 0x04 },
                    Status = "Active"
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                await repository.Add(userCredential);

                // Update status
                userCredential.Status = "Inactive";

                // Act
                var updatedUserCredential = await repository.Update(userCredential);

                // Assert
                Assert.IsNotNull(updatedUserCredential);
                Assert.AreEqual("Inactive", updatedUserCredential.Status);
            }
        }

        [Test]
        public void UpdateUserCredential_UserCredentialNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserCredentialRepository(context);
                var invalidUserCredential = new UserCredential
                {
                    Email = "nonexistent@example.com",
                    PasswordHashKey = new byte[] { 0x01, 0x02 },
                    Password = new byte[] { 0x03, 0x04 },
                    Status = "Active"
                };

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchUserCredentialFoundException>(() => repository.Update(invalidUserCredential));
                Assert.AreEqual("No such user credential found", ex.Message);
            }
        }
    }
}
