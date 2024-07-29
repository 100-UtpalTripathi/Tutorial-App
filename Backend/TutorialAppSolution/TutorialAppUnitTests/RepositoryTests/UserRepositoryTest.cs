using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.User;
using TutorialApp.Models;
using TutorialApp.Repositories;

namespace TutorialAppUnitTests.RepositoryTests
{
    public class UserRepositoryTest
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
        public async Task AddUserAsync_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserRepository(context);
                var user = new User
                {
                    Email = "test@example.com",
                    Name = "Test User",
                    Dob = new DateTime(1990, 1, 1),
                    Phone = "1234567890",
                    ImageURI = "http://example.com/image.jpg",
                    Role = "Student"
                };

                // Act
                var addedUser = await repository.Add(user);

                // Assert
                Assert.IsNotNull(addedUser);
                Assert.AreEqual(user.Email, addedUser.Email);
            }
        }

        [Test]
        public async Task DeleteUserByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserRepository(context);
                var user = new User
                {
                    Email = "test@example.com",
                    Name = "Test User",
                    Dob = new DateTime(1990, 1, 1),
                    Phone = "1234567890",
                    ImageURI = "http://example.com/image.jpg",
                    Role = "Student"
                };

                var addedUser = await repository.Add(user);

                // Act
                var deletedUser = await repository.DeleteByKey(addedUser.Email);

                // Assert
                Assert.IsNotNull(deletedUser);
                Assert.AreEqual(addedUser.Email, deletedUser.Email);
            }
        }

        [Test]
        public void DeleteUserByKey_UserNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserRepository(context);

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchUserFoundException>(() => repository.DeleteByKey("nonexistent@example.com"));
                Assert.AreEqual("No such user found", ex.Message);
            }
        }

        [Test]
        public async Task GetUserByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserRepository(context);
                var user = new User
                {
                    Email = "test@example.com",
                    Name = "Test User",
                    Dob = new DateTime(1990, 1, 1),
                    Phone = "1234567890",
                    ImageURI = "http://example.com/image.jpg",
                    Role = "Student"
                };

                var addedUser = await repository.Add(user);

                // Act
                var fetchedUser = await repository.GetByKey(addedUser.Email);

                // Assert
                Assert.IsNotNull(fetchedUser);
                Assert.AreEqual(addedUser.Email, fetchedUser.Email);
            }
        }

        [Test]
        public async Task GetUserByKey_UserNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserRepository(context);

                // Act
                var user = await repository.GetByKey("nonexistent@example.com");

                // Assert
                Assert.IsNull(user);
            }
        }

        [Test]
        public async Task GetUsers_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserRepository(context);
                var user1 = new User
                {
                    Email = "user1@example.com",
                    Name = "User One",
                    Dob = new DateTime(1985, 5, 15),
                    Phone = "1234567890",
                    ImageURI = "http://example.com/image1.jpg",
                    Role = "Student"
                };
                var user2 = new User
                {
                    Email = "user2@example.com",
                    Name = "User Two",
                    Dob = new DateTime(1988, 8, 20),
                    Phone = "0987654321",
                    ImageURI = "http://example.com/image2.jpg",
                    Role = "Instructor"
                };

                await repository.Add(user1);
                await repository.Add(user2);

                // Act
                var users = await repository.Get();

                // Assert
                Assert.IsNotNull(users);
                Assert.AreEqual(2, users.Count());
            }
        }

        [Test]
        public async Task UpdateUser_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserRepository(context);
                var user = new User
                {
                    Email = "test@example.com",
                    Name = "Test User",
                    Dob = new DateTime(1990, 1, 1),
                    Phone = "1234567890",
                    ImageURI = "http://example.com/image.jpg",
                    Role = "Student"
                };

                var addedUser = await repository.Add(user);

                addedUser.Name = "Updated User Name";
                addedUser.Phone = "1112223333";

                // Act
                var updatedUser = await repository.Update(addedUser);

                // Assert
                Assert.IsNotNull(updatedUser);
                Assert.AreEqual("Updated User Name", updatedUser.Name);
                Assert.AreEqual("1112223333", updatedUser.Phone);
            }
        }

        [Test]
        public void UpdateUser_UserNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new UserRepository(context);
                var invalidUser = new User
                {
                    Email = "nonexistent@example.com",
                    Name = "Non-existent User",
                    Dob = new DateTime(1990, 1, 1),
                    Phone = "1234567890",
                    ImageURI = "http://example.com/image.jpg",
                    Role = "Student"
                };

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchUserFoundException>(() => repository.Update(invalidUser));
                Assert.AreEqual("No such user found", ex.Message);
            }
        }
    }
}
