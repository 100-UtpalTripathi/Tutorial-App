using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Enrollment;
using TutorialApp.Models;
using TutorialApp.Repositories;

namespace TutorialAppUnitTests.RepositoryTests
{
    public class EnrollmentRepositoryTest
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
        public async Task AddEnrollmentAsync_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new EnrollmentRepository(context);
                var enrollment = new Enrollment
                {
                    UserEmail = "user@example.com",
                    CourseId = 1,
                    Status = "Registered",
                    EnrollmentDate = DateTime.UtcNow
                };

                // Act
                var addedEnrollment = await repository.Add(enrollment);

                // Assert
                Assert.IsNotNull(addedEnrollment);
                Assert.AreEqual(enrollment.UserEmail, addedEnrollment.UserEmail);
            }
        }

        [Test]
        public async Task DeleteEnrollmentByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new EnrollmentRepository(context);
                var enrollment = new Enrollment
                {
                    UserEmail = "user@example.com",
                    CourseId = 1,
                    Status = "Registered",
                    EnrollmentDate = DateTime.UtcNow
                };

                var addedEnrollment = await repository.Add(enrollment);

                // Act
                var deletedEnrollment = await repository.DeleteByKey(addedEnrollment.EnrollmentId);

                // Assert
                Assert.IsNotNull(deletedEnrollment);
                Assert.AreEqual(addedEnrollment.EnrollmentId, deletedEnrollment.EnrollmentId);
            }
        }

        [Test]
        public void DeleteEnrollmentByKey_EnrollmentNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new EnrollmentRepository(context);

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchEnrollmentFoundException>(() => repository.DeleteByKey(100));
                Assert.AreEqual("No such enrollment found", ex.Message);
            }
        }

        [Test]
        public async Task GetEnrollmentByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new EnrollmentRepository(context);
                var enrollment = new Enrollment
                {
                    UserEmail = "user@example.com",
                    CourseId = 1,
                    Status = "Registered",
                    EnrollmentDate = DateTime.UtcNow
                };

                var addedEnrollment = await repository.Add(enrollment);

                // Act
                var fetchedEnrollment = await repository.GetByKey(addedEnrollment.EnrollmentId);

                // Assert
                Assert.IsNotNull(fetchedEnrollment);
                Assert.AreEqual(addedEnrollment.EnrollmentId, fetchedEnrollment.EnrollmentId);
            }
        }

        [Test]
        public async Task GetEnrollmentByKey_EnrollmentNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new EnrollmentRepository(context);

                // Act
                var enrollment = await repository.GetByKey(100); // Assuming ID 100 does not exist

                // Assert
                Assert.IsNull(enrollment);
            }
        }

        [Test]
        public async Task GetEnrollments_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new EnrollmentRepository(context);
                var enrollment1 = new Enrollment
                {
                    UserEmail = "user1@example.com",
                    CourseId = 1,
                    Status = "Registered",
                    EnrollmentDate = DateTime.UtcNow
                };
                var enrollment2 = new Enrollment
                {
                    UserEmail = "user2@example.com",
                    CourseId = 2,
                    Status = "Registered",
                    EnrollmentDate = DateTime.UtcNow
                };

                await repository.Add(enrollment1);
                await repository.Add(enrollment2);

                // Act
                var enrollments = await repository.Get();

                // Assert
                Assert.IsNotNull(enrollments);
                Assert.AreEqual(2, enrollments.Count());
            }
        }

        [Test]
        public async Task UpdateEnrollment_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new EnrollmentRepository(context);
                var enrollment = new Enrollment
                {
                    UserEmail = "user@example.com",
                    CourseId = 1,
                    Status = "Registered",
                    EnrollmentDate = DateTime.UtcNow
                };

                var addedEnrollment = await repository.Add(enrollment);

                addedEnrollment.Status = "Completed";
                addedEnrollment.CompletionDate = DateTime.UtcNow;

                // Act
                var updatedEnrollment = await repository.Update(addedEnrollment);

                // Assert
                Assert.IsNotNull(updatedEnrollment);
                Assert.AreEqual("Completed", updatedEnrollment.Status);
            }
        }

        [Test]
        public void UpdateEnrollment_EnrollmentNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new EnrollmentRepository(context);
                var invalidEnrollment = new Enrollment
                {
                    EnrollmentId = 100,
                    UserEmail = "user@example.com",
                    CourseId = 1,
                    Status = "Registered",
                    EnrollmentDate = DateTime.UtcNow
                };

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchEnrollmentFoundException>(() => repository.Update(invalidEnrollment));
                Assert.AreEqual("No such enrollment found", ex.Message);
            }
        }
    }
}
