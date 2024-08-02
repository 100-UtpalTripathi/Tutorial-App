using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Course;
using TutorialApp.Models;
using TutorialApp.Repositories;

namespace TutorialAppUnitTests.RepositoryTests
{
    public class CourseRepositoryTest
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
        public async Task AddCourseAsync_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CourseRepository(context);
                var course = new Course
                {
                    Title = "Test Course",
                    Description = "Test Description",
                    CategoryName = "Web Development",
                    Price = 100,
                    CourseImageUrl = "http://example.com/image.jpg",
                    InstructorName = "John Doe",
                    CourseURL = "http://example.com/course"
                };

                // Act
                var addedCourse = await repository.Add(course);

                // Assert
                Assert.IsNotNull(addedCourse);
                Assert.AreEqual(course.Title, addedCourse.Title);
            }
        }

        [Test]
        public async Task DeleteCourseByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CourseRepository(context);
                var course = new Course
                {
                    Title = "Test Course",
                    Description = "Test Description",
                    CategoryName= "Web Development",
                    Price = 100,
                    CourseImageUrl = "http://example.com/image.jpg",
                    InstructorName = "John Doe",
                    CourseURL = "http://example.com/course"
                };

                var addedCourse = await repository.Add(course);

                // Act
                var deletedCourse = await repository.DeleteByKey(addedCourse.CourseId);

                // Assert
                Assert.IsNotNull(deletedCourse);
                Assert.AreEqual(addedCourse.CourseId, deletedCourse.CourseId);
            }
        }

        [Test]
        public void DeleteCourseByKey_CourseNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CourseRepository(context);

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchCourseFoundException>(() => repository.DeleteByKey(100));
                Assert.AreEqual("No such course found", ex.Message);
            }
        }

        [Test]
        public async Task GetCourseByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CourseRepository(context);
                var course = new Course
                {
                    Title = "Test Course",
                    Description = "Test Description",
                    CategoryName = "Web Development",
                    Price = 100,
                    CourseImageUrl = "http://example.com/image.jpg",
                    InstructorName = "John Doe",
                    CourseURL = "http://example.com/course"
                };

                var addedCourse = await repository.Add(course);

                // Act
                var fetchedCourse = await repository.GetByKey(addedCourse.CourseId);

                // Assert
                Assert.IsNotNull(fetchedCourse);
                Assert.AreEqual(addedCourse.CourseId, fetchedCourse.CourseId);
            }
        }

        [Test]
        public async Task GetCourseByKey_CourseNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CourseRepository(context);

                // Act
                var course = await repository.GetByKey(100); // Assuming ID 100 does not exist

                // Assert
                Assert.IsNull(course);
            }
        }

        [Test]
        public async Task GetCourses_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CourseRepository(context);
                var course1 = new Course
                {
                    Title = "Test Course 1",
                    Description = "Test Description 1",
                    CategoryName = "Web Development",
                    Price = 100,
                    CourseImageUrl = "http://example.com/image1.jpg",
                    InstructorName = "John Doe 1",
                    CourseURL = "http://example.com/course1"
                };
                var course2 = new Course
                {
                    Title = "Test Course 2",
                    Description = "Test Description 2",
                    CategoryName = "Web Development",
                    Price = 200,
                    CourseImageUrl = "http://example.com/image2.jpg",
                    InstructorName = "John Doe 2",
                    CourseURL = "http://example.com/course2"
                };

                await repository.Add(course1);
                await repository.Add(course2);

                // Act
                var courses = await repository.Get();

                // Assert
                Assert.IsNotNull(courses);
                Assert.AreEqual(2, courses.Count());
            }
        }

        [Test]
        public async Task UpdateCourse_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CourseRepository(context);
                var course = new Course
                {
                    Title = "Original Course",
                    Description = "Original Description",
                    CategoryName = "Web Development",
                    Price = 100,
                    CourseImageUrl = "http://example.com/image.jpg",
                    InstructorName = "John Doe",
                    CourseURL = "http://example.com/course"
                };

                var addedCourse = await repository.Add(course);

                addedCourse.Title = "Updated Course";
                addedCourse.Description = "Updated Description";

                // Act
                var updatedCourse = await repository.Update(addedCourse);

                // Assert
                Assert.IsNotNull(updatedCourse);
                Assert.AreEqual("Updated Course", updatedCourse.Title);
            }
        }

        [Test]
        public void UpdateCourse_CourseNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CourseRepository(context);
                var invalidCourse = new Course
                {
                    CourseId = 100,
                    Title = "Nonexistent Course",
                    Description = "Nonexistent Description",
                    CategoryId = 1,
                    Price = 100,
                    CourseImageUrl = "http://example.com/image.jpg",
                    InstructorName = "John Doe"
                };

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchCourseFoundException>(() => repository.Update(invalidCourse));
                Assert.AreEqual("No such course found", ex.Message);
            }
        }
    }
}
