using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Wishlist;
using TutorialApp.Models;
using TutorialApp.Repositories;

namespace TutorialAppUnitTests.RepositoryTests
{
    public class WishlistRepositoryTest
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
        public async Task AddWishlistAsync_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new WishlistRepository(context);
                var wishlist = new Wishlist
                {
                    UserEmail = "testuser@example.com",
                    CourseId = 1
                };

                // Act
                var addedWishlist = await repository.Add(wishlist);

                // Assert
                Assert.IsNotNull(addedWishlist);
                Assert.AreEqual(wishlist.UserEmail, addedWishlist.UserEmail);
                Assert.AreEqual(wishlist.CourseId, addedWishlist.CourseId);
            }
        }

        [Test]
        public async Task DeleteWishlistByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new WishlistRepository(context);
                var wishlist = new Wishlist
                {
                    UserEmail = "testuser@example.com",
                    CourseId = 1
                };

                var addedWishlist = await repository.Add(wishlist);

                // Act
                var deletedWishlist = await repository.DeleteByKey(addedWishlist.WishlistId);

                // Assert
                Assert.IsNotNull(deletedWishlist);
                Assert.AreEqual(addedWishlist.WishlistId, deletedWishlist.WishlistId);
            }
        }

        [Test]
        public void DeleteWishlistByKey_WishlistNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new WishlistRepository(context);

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchWishlistFoundException>(() => repository.DeleteByKey(9999));
                Assert.AreEqual("No such wishlist found", ex.Message);
            }
        }

        [Test]
        public async Task GetWishlistByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new WishlistRepository(context);
                var wishlist = new Wishlist
                {
                    UserEmail = "testuser@example.com",
                    CourseId = 1
                };

                var addedWishlist = await repository.Add(wishlist);

                // Act
                var fetchedWishlist = await repository.GetByKey(addedWishlist.WishlistId);

                // Assert
                Assert.IsNotNull(fetchedWishlist);
                Assert.AreEqual(addedWishlist.WishlistId, fetchedWishlist.WishlistId);
            }
        }

        [Test]
        public async Task GetWishlistByKey_WishlistNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new WishlistRepository(context);

                // Act
                var wishlist = await repository.GetByKey(9999);

                // Assert
                Assert.IsNull(wishlist);
            }
        }

        [Test]
        public async Task GetWishlists_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new WishlistRepository(context);
                var wishlist1 = new Wishlist
                {
                    UserEmail = "user1@example.com",
                    CourseId = 1
                };
                var wishlist2 = new Wishlist
                {
                    UserEmail = "user2@example.com",
                    CourseId = 2
                };

                await repository.Add(wishlist1);
                await repository.Add(wishlist2);

                // Act
                var wishlists = await repository.Get();

                // Assert
                Assert.IsNotNull(wishlists);
                Assert.AreEqual(2, wishlists.Count());
            }
        }

        [Test]
        public async Task UpdateWishlist_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new WishlistRepository(context);
                var wishlist = new Wishlist
                {
                    UserEmail = "testuser@example.com",
                    CourseId = 1
                };

                var addedWishlist = await repository.Add(wishlist);

                addedWishlist.CourseId = 2;

                // Act
                var updatedWishlist = await repository.Update(addedWishlist);

                // Assert
                Assert.IsNotNull(updatedWishlist);
                Assert.AreEqual(2, updatedWishlist.CourseId);
            }
        }

        [Test]
        public void UpdateWishlist_WishlistNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new WishlistRepository(context);
                var invalidWishlist = new Wishlist
                {
                    WishlistId = 9999,
                    UserEmail = "nonexistent@example.com",
                    CourseId = 2
                };

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchWishlistFoundException>(() => repository.Update(invalidWishlist));
                Assert.AreEqual("No such wishlist found", ex.Message);
            }
        }
    }
}
