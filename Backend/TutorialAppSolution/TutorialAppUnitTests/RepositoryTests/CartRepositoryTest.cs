using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Cart;
using TutorialApp.Models;
using TutorialApp.Repositories;

namespace TutorialAppUnitTests.RepositoryTests
{
    public class CartRepositoryTest
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
        public async Task AddToCartAsync_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CartRepository(context);
                var cart = new Cart
                {
                    UserEmail = "user@example.com",
                    CourseId = 1,
                    Price = 100
                };

                // Act
                var addedCart = await repository.Add(cart);

                // Assert
                Assert.IsNotNull(addedCart);
                Assert.AreEqual(cart.UserEmail, addedCart.UserEmail);
                Assert.AreEqual(cart.CourseId, addedCart.CourseId);
                Assert.AreEqual(cart.Price, addedCart.Price);
            }
        }

        [Test]
        public async Task RemoveFromCartByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CartRepository(context);
                var cart = new Cart
                {
                    UserEmail = "user@example.com",
                    CourseId = 1,
                    Price = 100
                };

                var addedCart = await repository.Add(cart);

                // Act
                var removedCart = await repository.DeleteByKey(addedCart.CartId);

                // Assert
                Assert.IsNotNull(removedCart);
                Assert.AreEqual(addedCart.CartId, removedCart.CartId);
            }
        }

        [Test]
        public async Task RemoveFromCartByKey_CartNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CartRepository(context);

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchCartFoundException>(() => repository.DeleteByKey(100));
                Assert.AreEqual("No Cart found with given ID!", ex.Message);
            }
        }

        [Test]
        public async Task GetCartByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CartRepository(context);
                var cart = new Cart
                {
                    UserEmail = "user@example.com",
                    CourseId = 1,
                    Price = 100
                };

                var addedCart = await repository.Add(cart);

                // Act
                var retrievedCart = await repository.GetByKey(addedCart.CartId);

                // Assert
                Assert.IsNotNull(retrievedCart);
                Assert.AreEqual(addedCart.CartId, retrievedCart.CartId);
            }
        }

        [Test]
        public async Task GetCartByKey_CartNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CartRepository(context);

                // Act
                var cart = await repository.GetByKey(100); // Assuming ID 100 does not exist
                Assert.IsNull(cart);
            }
        }

        [Test]
        public async Task GetCarts_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CartRepository(context);
                var cart1 = new Cart
                {
                    UserEmail = "user1@example.com",
                    CourseId = 1,
                    Price = 100
                };
                var cart2 = new Cart
                {
                    UserEmail = "user2@example.com",
                    CourseId = 2,
                    Price = 200
                };

                await repository.Add(cart1);
                await repository.Add(cart2);

                // Act
                var carts = await repository.Get();

                // Assert
                Assert.IsNotNull(carts);
                Assert.AreEqual(2, carts.Count());
            }
        }

        [Test]
        public async Task UpdateCart_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CartRepository(context);
                var cart = new Cart
                {
                    UserEmail = "user@example.com",
                    CourseId = 1,
                    Price = 100
                };

                var addedCart = await repository.Add(cart);
                addedCart.Price = 150;

                // Act
                var updatedCart = await repository.Update(addedCart);

                // Assert
                Assert.IsNotNull(updatedCart);
                Assert.AreEqual(addedCart.Price, updatedCart.Price);
            }
        }

        [Test]
        public async Task UpdateCart_CartNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new CartRepository(context);
                var invalidCart = new Cart
                {
                    CartId = 100,
                    UserEmail = "user@example.com",
                    CourseId = 1,
                    Price = 100
                };

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchCartFoundException>(() => repository.Update(invalidCart));
                Assert.AreEqual("No Cart found with given ID!", ex.Message);
            }
        }
    }
}
