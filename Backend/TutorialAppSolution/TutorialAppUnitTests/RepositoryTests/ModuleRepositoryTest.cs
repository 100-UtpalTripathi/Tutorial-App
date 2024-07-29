using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Module;
using TutorialApp.Models;
using TutorialApp.Repositories;

namespace TutorialAppUnitTests.RepositoryTests
{
    public class ModuleRepositoryTest
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
        public async Task AddModuleAsync_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new ModuleRepository(context);
                var module = new Module
                {
                    CourseId = 1,
                    Title = "Module 1",
                    Content = "Content for Module 1"
                };

                // Act
                var addedModule = await repository.Add(module);

                // Assert
                Assert.IsNotNull(addedModule);
                Assert.AreEqual(module.Title, addedModule.Title);
            }
        }

        [Test]
        public async Task DeleteModuleByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new ModuleRepository(context);
                var module = new Module
                {
                    CourseId = 1,
                    Title = "Module 1",
                    Content = "Content for Module 1"
                };

                var addedModule = await repository.Add(module);

                // Act
                var deletedModule = await repository.DeleteByKey(addedModule.ModuleId);

                // Assert
                Assert.IsNotNull(deletedModule);
                Assert.AreEqual(addedModule.ModuleId, deletedModule.ModuleId);
            }
        }

        [Test]
        public void DeleteModuleByKey_ModuleNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new ModuleRepository(context);

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchModuleFoundException>(() => repository.DeleteByKey(100));
                Assert.AreEqual("No such module found", ex.Message);
            }
        }

        [Test]
        public async Task GetModuleByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new ModuleRepository(context);
                var module = new Module
                {
                    CourseId = 1,
                    Title = "Module 1",
                    Content = "Content for Module 1"
                };

                var addedModule = await repository.Add(module);

                // Act
                var fetchedModule = await repository.GetByKey(addedModule.ModuleId);

                // Assert
                Assert.IsNotNull(fetchedModule);
                Assert.AreEqual(addedModule.ModuleId, fetchedModule.ModuleId);
            }
        }

        [Test]
        public async Task GetModuleByKey_ModuleNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new ModuleRepository(context);

                // Act
                var module = await repository.GetByKey(100); // Assuming ID 100 does not exist

                // Assert
                Assert.IsNull(module);
            }
        }

        [Test]
        public async Task GetModules_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new ModuleRepository(context);
                var module1 = new Module
                {
                    CourseId = 1,
                    Title = "Module 1",
                    Content = "Content for Module 1"
                };
                var module2 = new Module
                {
                    CourseId = 2,
                    Title = "Module 2",
                    Content = "Content for Module 2"
                };

                await repository.Add(module1);
                await repository.Add(module2);

                // Act
                var modules = await repository.Get();

                // Assert
                Assert.IsNotNull(modules);
                Assert.AreEqual(2, modules.Count());
            }
        }

        [Test]
        public async Task UpdateModule_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new ModuleRepository(context);
                var module = new Module
                {
                    CourseId = 1,
                    Title = "Module 1",
                    Content = "Content for Module 1"
                };

                var addedModule = await repository.Add(module);

                addedModule.Title = "Updated Module 1";
                addedModule.Content = "Updated content for Module 1";

                // Act
                var updatedModule = await repository.Update(addedModule);

                // Assert
                Assert.IsNotNull(updatedModule);
                Assert.AreEqual("Updated Module 1", updatedModule.Title);
            }
        }

        [Test]
        public void UpdateModule_ModuleNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new ModuleRepository(context);
                var invalidModule = new Module
                {
                    ModuleId = 100,
                    CourseId = 1,
                    Title = "Module 1",
                    Content = "Content for Module 1"
                };

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchModuleFoundException>(() => repository.Update(invalidModule));
                Assert.AreEqual("No such module found", ex.Message);
            }
        }
    }
}
