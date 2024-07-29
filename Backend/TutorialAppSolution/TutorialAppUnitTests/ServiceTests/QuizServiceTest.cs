using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Exceptions.Quiz;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Quiz;
using TutorialApp.Services;

namespace TutorialAppUnitTests.ServiceTests
{
    [TestFixture]
    public class QuizServiceTests
    {
        private Mock<IRepository<int, Quiz>> _mockQuizRepository;
        private Mock<ILogger<QuizService>> _mockLogger;
        private QuizService _quizService;

        [SetUp]
        public void Setup()
        {
            _mockQuizRepository = new Mock<IRepository<int, Quiz>>();
            _mockLogger = new Mock<ILogger<QuizService>>();
            _quizService = new QuizService(_mockQuizRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetQuizByCourseIdAsync_ShouldReturnQuiz_WhenQuizExists()
        {
            // Arrange
            var courseId = 1;
            var quiz = new Quiz { CourseId = courseId, Questions = new List<Question>() };
            _mockQuizRepository.Setup(repo => repo.Get()).ReturnsAsync(new List<Quiz> { quiz });

            // Act
            var result = await _quizService.GetQuizByCourseIdAsync(courseId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(courseId, result.CourseId);
            Assert.IsNotNull(result.Questions);
        }

        [Test]
        public void GetQuizByCourseIdAsync_ShouldThrowNoSuchQuizFoundException_WhenNoQuizExists()
        {
            // Arrange
            _mockQuizRepository.Setup(repo => repo.Get()).ReturnsAsync(new List<Quiz>());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchQuizFoundException>(async () => await _quizService.GetQuizByCourseIdAsync(2));
        }

        [Test]
        public async Task CreateQuizAsync_ShouldCreateAndReturnQuiz_WhenValidDTOProvided()
        {
            // Arrange
            var quizDTO = new QuizDTO { CourseId = 1, Title = "Sample Quiz" };
            var quiz = new Quiz { CourseId = quizDTO.CourseId, Title = quizDTO.Title };
            _mockQuizRepository.Setup(repo => repo.Add(It.IsAny<Quiz>())).ReturnsAsync(quiz);

            // Act
            var result = await _quizService.CreateQuizAsync(quizDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(quizDTO.CourseId, result.CourseId);
            Assert.AreEqual(quizDTO.Title, result.Title);
        }

        [Test]
        public void CreateQuizAsync_ShouldThrowQuizCreationFailedException_WhenExceptionOccurs()
        {
            // Arrange
            var quizDTO = new QuizDTO { CourseId = 1, Title = "Sample Quiz" };
            _mockQuizRepository.Setup(repo => repo.Add(It.IsAny<Quiz>())).ThrowsAsync(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<QuizCreationFailedException>(async () => await _quizService.CreateQuizAsync(quizDTO));
        }

        [Test]
        public async Task DeleteQuizAsync_ShouldDeleteAndReturnQuiz_WhenQuizExists()
        {
            // Arrange
            var quizId = 1;
            var quiz = new Quiz { CourseId = 1, Title = "Sample Quiz" };
            _mockQuizRepository.Setup(repo => repo.DeleteByKey(quizId)).ReturnsAsync(quiz);

            // Act
            var result = await _quizService.DeleteQuizAsync(quizId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(quizId, result.CourseId);
        }

        [Test]
        public void DeleteQuizAsync_ShouldThrowQuizDeletionFailedException_WhenExceptionOccurs()
        {
            // Arrange
            var quizId = 1;
            _mockQuizRepository.Setup(repo => repo.DeleteByKey(quizId)).ThrowsAsync(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<QuizDeletionFailedException>(async () => await _quizService.DeleteQuizAsync(quizId));
        }
    }
}
