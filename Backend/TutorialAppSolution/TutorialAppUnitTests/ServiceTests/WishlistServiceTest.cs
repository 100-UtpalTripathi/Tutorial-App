using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TutorialApp.Exceptions.Wishlist;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Wishlist;
using TutorialApp.Repositories;
using TutorialApp.Services;

namespace TutorialAppUnitTests.ServiceTests
{
    [TestFixture]
    public class WishlistServiceTests
    {
        private Mock<IRepository<int, Wishlist>> _mockWishlistRepo;
        private Mock<IRepository<int, Course>> _mockCourseRepo;
        private Mock<ILogger<WishlistService>> _mockLogger;
        private WishlistService _wishlistService;

        [SetUp]
        public void Setup()
        {
            _mockWishlistRepo = new Mock<IRepository<int, Wishlist>>();
            _mockCourseRepo = new Mock<IRepository<int, Course>>();
            _mockLogger = new Mock<ILogger<WishlistService>>();
            _wishlistService = new WishlistService(_mockWishlistRepo.Object, _mockCourseRepo.Object, _mockLogger.Object);
        }

        [Test]
        public async Task AddToWishlistAsync_ShouldAddWishlist_WhenDataIsValid()
        {
            // Arrange
            var wishlistDTO = new WishListDTO { UserEmail = "test@example.com", CourseId = 1 };
            var wishlist = new Wishlist { UserEmail = wishlistDTO.UserEmail, CourseId = wishlistDTO.CourseId };

            _mockWishlistRepo.Setup(repo => repo.Add(It.IsAny<Wishlist>()))
                .ReturnsAsync(wishlist);

            // Act
            var result = await _wishlistService.AddToWishlistAsync(wishlistDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(wishlistDTO.UserEmail, result.UserEmail);
            Assert.AreEqual(wishlistDTO.CourseId, result.CourseId);
        }

        [Test]
        public void AddToWishlistAsync_ShouldThrowException_WhenAddFails()
        {
            // Arrange
            var wishlistDTO = new WishListDTO { UserEmail = "test@example.com", CourseId = 1 };
            _mockWishlistRepo.Setup(repo => repo.Add(It.IsAny<Wishlist>()))
                .ThrowsAsync(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<UnableToAddToWishlistException>(async () =>
                await _wishlistService.AddToWishlistAsync(wishlistDTO));
        }

        [Test]
        public async Task RemoveFromWishlistAsync_ShouldRemoveWishlist_WhenWishlistExists()
        {
            // Arrange
            var wishlistDTO = new WishListDTO { UserEmail = "test@example.com", CourseId = 1 };
            var wishlist = new Wishlist { WishlistId = 1, UserEmail = wishlistDTO.UserEmail, CourseId = wishlistDTO.CourseId };

            _mockWishlistRepo.Setup(repo => repo.Get())
                .ReturnsAsync(new List<Wishlist> { wishlist });

            _mockWishlistRepo.Setup(repo => repo.DeleteByKey(It.IsAny<int>()))
                .ReturnsAsync(wishlist);

            // Act
            var result = await _wishlistService.RemoveFromWishlistAsync(wishlistDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(wishlist.WishlistId, result.WishlistId);
        }

        [Test]
        public void RemoveFromWishlistAsync_ShouldThrowException_WhenWishlistNotFound()
        {
            // Arrange
            var wishlistDTO = new WishListDTO { UserEmail = "test@example.com", CourseId = 1 };
            _mockWishlistRepo.Setup(repo => repo.Get())
                .ReturnsAsync(new List<Wishlist>());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchWishlistFoundException>(async () =>
                await _wishlistService.RemoveFromWishlistAsync(wishlistDTO));
        }

        [Test]
        public async Task GetWishlistedCoursesByUserAsync_ShouldReturnCourses_WhenWishlistsExist()
        {
            // Arrange
            var userEmail = "test@example.com";
            var wishlist = new Wishlist { UserEmail = userEmail, CourseId = 1 };
            var course = new Course { CourseId = 1, Title = "Course 1" };

            _mockWishlistRepo.Setup(repo => repo.Get())
                .ReturnsAsync(new List<Wishlist> { wishlist });

            _mockCourseRepo.Setup(repo => repo.GetByKey(It.IsAny<int>()))
                .ReturnsAsync(course);

            // Act
            var result = await _wishlistService.GetWishlistedCoursesByUserAsync(userEmail);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(course.Title, result.First().Title);
        }

        [Test]
        public void GetWishlistedCoursesByUserAsync_ShouldThrowException_WhenNoWishlistsFound()
        {
            // Arrange
            var userEmail = "test@example.com";
            _mockWishlistRepo.Setup(repo => repo.Get())
                .ReturnsAsync(new List<Wishlist>());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchWishlistFoundException>(async () =>
                await _wishlistService.GetWishlistedCoursesByUserAsync(userEmail));
        }
    }
}
