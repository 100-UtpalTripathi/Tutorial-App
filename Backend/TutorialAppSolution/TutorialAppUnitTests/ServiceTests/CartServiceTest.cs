using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Exceptions.Cart;
using TutorialApp.Exceptions.Course;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Cart;
using TutorialApp.Services;

namespace TutorialAppUnitTests.ServiceTests
{
    [TestFixture]
    public class CartServiceTests
    {
        private Mock<IRepository<int, Cart>> _mockCartRepository;
        private Mock<IRepository<int, Course>> _mockCourseRepository;
        private Mock<IRepository<int, Enrollment>> _mockEnrollmentRepository;
        private Mock<ILogger<CartService>> _mockLogger;
        private CartService _cartService;

        [SetUp]
        public void Setup()
        {
            _mockCartRepository = new Mock<IRepository<int, Cart>>();
            _mockCourseRepository = new Mock<IRepository<int, Course>>();
            _mockEnrollmentRepository = new Mock<IRepository<int, Enrollment>>();
            _mockLogger = new Mock<ILogger<CartService>>();
            _cartService = new CartService(_mockCartRepository.Object, _mockCourseRepository.Object, _mockEnrollmentRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task AddToCartAsync_ShouldAddItemToCart_WhenValidDTOProvided()
        {
            // Arrange
            var cartDTO = new CartDTO { UserEmail = "user@example.com", CourseId = 1 };
            var existingCartItems = new List<Cart>();
            var enrollments = new List<Enrollment>
            {
                new Enrollment { UserEmail = "user@example.com", Status = "Completed" },
                new Enrollment { UserEmail = "user@example.com", Status = "Completed" },
                new Enrollment { UserEmail = "user@example.com", Status = "Completed" }
            };
            var course = new Course { CourseId = 1, Price = 100m };
            var cartItem = new Cart
            {
                UserEmail = cartDTO.UserEmail,
                CourseId = cartDTO.CourseId,
                Price = course.Price * 0.9m // 10% discount applied
            };

            _mockCartRepository.Setup(repo => repo.Get()).ReturnsAsync(existingCartItems);
            _mockCourseRepository.Setup(repo => repo.GetByKey(cartDTO.CourseId)).ReturnsAsync(course);
            _mockEnrollmentRepository.Setup(repo => repo.Get()).ReturnsAsync(enrollments);
            _mockCartRepository.Setup(repo => repo.Add(It.IsAny<Cart>())).ReturnsAsync(cartItem);

            // Act
            var result = await _cartService.AddToCartAsync(cartDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cartDTO.UserEmail, result.UserEmail);
            Assert.AreEqual(cartDTO.CourseId, result.CourseId);
            Assert.AreEqual(course.Price * 0.9m, result.Price); // Check for discount
        }

        [Test]
        public void AddToCartAsync_ShouldThrowDuplicateItemAddingException_WhenItemAlreadyExists()
        {
            // Arrange
            var cartDTO = new CartDTO { UserEmail = "user@example.com", CourseId = 1 };
            var existingCartItems = new List<Cart>
            {
                new Cart { UserEmail = cartDTO.UserEmail, CourseId = cartDTO.CourseId }
            };
            _mockCartRepository.Setup(repo => repo.Get()).ReturnsAsync(existingCartItems);

            // Act & Assert
            Assert.ThrowsAsync<DuplicateItemAddingException>(async () => await _cartService.AddToCartAsync(cartDTO));
        }

        [Test]
        public void AddToCartAsync_ShouldThrowNoSuchCourseFoundException_WhenCourseNotFound()
        {
            // Arrange
            var cartDTO = new CartDTO { UserEmail = "user@example.com", CourseId = 1 };
            var existingCartItems = new List<Cart>();
            _mockCartRepository.Setup(repo => repo.Get()).ReturnsAsync(existingCartItems);
            _mockCourseRepository.Setup(repo => repo.GetByKey(cartDTO.CourseId)).ReturnsAsync((Course)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchCourseFoundException>(async () => await _cartService.AddToCartAsync(cartDTO));
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldRemoveItemFromCart_WhenItemExists()
        {
            // Arrange
            var cartDTO = new CartDTO { UserEmail = "user@example.com", CourseId = 1 };
            var cartItem = new Cart { CartId = 1, UserEmail = cartDTO.UserEmail, CourseId = cartDTO.CourseId };
            _mockCartRepository.Setup(repo => repo.Get()).ReturnsAsync(new List<Cart> { cartItem });
            _mockCartRepository.Setup(repo => repo.DeleteByKey(cartItem.CartId)).ReturnsAsync(cartItem);

            // Act
            var result = await _cartService.RemoveFromCartAsync(cartDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cartItem.CartId, result.CartId);
        }

        [Test]
        public void RemoveFromCartAsync_ShouldThrowNoSuchCartFoundException_WhenItemNotFound()
        {
            // Arrange
            var cartDTO = new CartDTO { UserEmail = "user@example.com", CourseId = 1 };
            _mockCartRepository.Setup(repo => repo.Get()).ReturnsAsync(new List<Cart>());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchCartFoundException>(async () => await _cartService.RemoveFromCartAsync(cartDTO));
        }

        [Test]
        public void RemoveFromCartAsync_ShouldThrowCartDeleteFailedException_WhenExceptionOccurs()
        {
            // Arrange
            var cartDTO = new CartDTO { UserEmail = "user@example.com", CourseId = 1 };
            var cartItem = new Cart { CartId = 1, UserEmail = cartDTO.UserEmail, CourseId = cartDTO.CourseId };
            _mockCartRepository.Setup(repo => repo.Get()).ReturnsAsync(new List<Cart> { cartItem });
            _mockCartRepository.Setup(repo => repo.DeleteByKey(cartItem.CartId)).ThrowsAsync(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<CartDeleteFailedException>(async () => await _cartService.RemoveFromCartAsync(cartDTO));
        }

        [Test]
        public async Task GetCartItemsByUserAsync_ShouldReturnCourses_WhenCartItemsExist()
        {
            // Arrange
            var userEmail = "user@example.com";
            var cartItems = new List<Cart>
            {
                new Cart { UserEmail = userEmail, CourseId = 1 },
                new Cart { UserEmail = userEmail, CourseId = 2 }
            };
            var courses = new List<Course>
            {
                new Course { CourseId = 1, Title = "Course 1" },
                new Course { CourseId = 2, Title = "Course 2" }
            };
            _mockCartRepository.Setup(repo => repo.Get()).ReturnsAsync(cartItems);
            _mockCourseRepository.Setup(repo => repo.GetByKey(It.IsAny<int>())).ReturnsAsync((int id) => courses.FirstOrDefault(c => c.CourseId == id));

            // Act
            var result = await _cartService.GetCartItemsByUserAsync(userEmail);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(c => c.Title == "Course 1"));
            Assert.IsTrue(result.Any(c => c.Title == "Course 2"));
        }

        [Test]
        public async Task GetCartItemsByUserAsync_ShouldReturnEmptyList_WhenNoCartItemsExist()
        {
            // Arrange
            var userEmail = "user@example.com";
            _mockCartRepository.Setup(repo => repo.Get()).ReturnsAsync(new List<Cart>());

            // Act
            var result = await _cartService.GetCartItemsByUserAsync(userEmail);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }
    }
}
