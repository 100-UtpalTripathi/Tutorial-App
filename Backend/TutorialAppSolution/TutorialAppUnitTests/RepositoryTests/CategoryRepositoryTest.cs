using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Category;
using TutorialApp.Models;
using TutorialApp.Repositories;

namespace TutorialAppUnitTests.RepositoryTests
{
    public class CategoryRepositoryTest
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
        public async Task AddCategoryAsync_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CategoryRepository(context);
                var category = new Category
                {
                    Name = "Test Category"
                };

                // Act
                var addedCategory = await repository.Add(category);

                // Assert
                Assert.IsNotNull(addedCategory);
                Assert.AreEqual(category.Name, addedCategory.Name);
            }
        }

        [Test]
        public async Task DeleteCategoryByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CategoryRepository(context);
                var category = new Category
                {
                    Name = "Test Category"
                };

                var addedCategory = await repository.Add(category);

                // Act
                var deletedCategory = await repository.DeleteByKey(addedCategory.CategoryId);

                // Assert
                Assert.IsNotNull(deletedCategory);
                Assert.AreEqual(addedCategory.CategoryId, deletedCategory.CategoryId);
            }
        }

        [Test]
        public void DeleteCategoryByKey_CategoryNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CategoryRepository(context);

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchCategoryFoundException>(() => repository.DeleteByKey(100));
                Assert.AreEqual("No Category found with given ID!", ex.Message);
            }
        }

        [Test]
        public async Task GetCategoryByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CategoryRepository(context);
                var category = new Category
                {
                    Name = "Test Category"
                };

                var addedCategory = await repository.Add(category);

                // Act
                var fetchedCategory = await repository.GetByKey(addedCategory.CategoryId);

                // Assert
                Assert.IsNotNull(fetchedCategory);
                Assert.AreEqual(addedCategory.CategoryId, fetchedCategory.CategoryId);
            }
        }

        [Test]
        public async Task GetCategoryByKey_CategoryNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CategoryRepository(context);

                // Act
                var category = await repository.GetByKey(100); // Assuming ID 100 does not exist

                // Assert
                Assert.IsNull(category);
            }
        }

        [Test]
        public async Task GetCategories_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CategoryRepository(context);
                var category1 = new Category
                {
                    Name = "Test Category 1"
                };
                var category2 = new Category
                {
                    Name = "Test Category 2"
                };

                await repository.Add(category1);
                await repository.Add(category2);

                // Act
                var categories = await repository.Get();

                // Assert
                Assert.IsNotNull(categories);
                Assert.AreEqual(2, categories.Count());
            }
        }

        [Test]
        public async Task UpdateCategory_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CategoryRepository(context);
                var category = new Category
                {
                    Name = "Original Category"
                };

                var addedCategory = await repository.Add(category);

                addedCategory.Name = "Updated Category";

                // Act
                var updatedCategory = await repository.Update(addedCategory);

                // Assert
                Assert.IsNotNull(updatedCategory);
                Assert.AreEqual("Updated Category", updatedCategory.Name);
            }
        }

        [Test]
        public void UpdateCategory_CategoryNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CategoryRepository(context);
                var invalidCategory = new Category
                {
                    CategoryId = 100,
                    Name = "Nonexistent Category"
                };

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchCategoryFoundException>(() => repository.Update(invalidCategory));
                Assert.AreEqual("No Category found with given ID!", ex.Message);
            }
        }
    }
}
