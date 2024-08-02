using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Exceptions.Question;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Question;
using TutorialApp.Services;

namespace TutorialAppUnitTests.ServiceTests
{
    [TestFixture]
    public class QuestionServiceTests
    {
        private Mock<IRepository<int, Question>> _mockQuestionRepository;
        private Mock<ILogger<QuestionService>> _mockLogger;
        private QuestionService _questionService;

        [SetUp]
        public void Setup()
        {
            _mockQuestionRepository = new Mock<IRepository<int, Question>>();
            _mockLogger = new Mock<ILogger<QuestionService>>();
            _questionService = new QuestionService(_mockQuestionRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task AddQuestion_ShouldReturnAddedQuestion_WhenValidDTOProvided()
        {
            // Arrange
            var questionDTO = new QuestionDTO
            {
                QuizId = 1,
                Text = "Sample Question",
                OptionA = "A",
                OptionB = "B",
                OptionC = "C",
                OptionD = "D",
                CorrectAnswer = "A"
            };
            var question = new Question
            {
                QuestionId = 1,
                QuizId = questionDTO.QuizId,
                Text = questionDTO.Text,
                OptionA = questionDTO.OptionA,
                OptionB = questionDTO.OptionB,
                OptionC = questionDTO.OptionC,
                OptionD = questionDTO.OptionD,
                CorrectAnswer = questionDTO.CorrectAnswer
            };
            _mockQuestionRepository.Setup(repo => repo.Add(It.IsAny<Question>())).ReturnsAsync(question);

            // Act
            var result = await _questionService.AddQuestion(questionDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(questionDTO.QuizId, result.QuizId);
            Assert.AreEqual(questionDTO.Text, result.Text);
        }

        [Test]
        public void AddQuestion_ShouldThrowCreateQuestionFailedException_WhenExceptionOccurs()
        {
            // Arrange
            var questionDTO = new QuestionDTO
            {
                QuizId = 1,
                Text = "Sample Question"
            };
            _mockQuestionRepository.Setup(repo => repo.Add(It.IsAny<Question>())).ThrowsAsync(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<CreateQuestionFailedException>(async () => await _questionService.AddQuestion(questionDTO));
        }

        [Test]
        public async Task UpdateQuestion_ShouldReturnUpdatedQuestion_WhenQuestionExists()
        {
            // Arrange
            var id = 1;
            var questionDTO = new QuestionDTO
            {
                QuizId = 1,
                Text = "Updated Question",
                OptionA = "A",
                OptionB = "B",
                OptionC = "C",
                OptionD = "D",
                CorrectAnswer = "B"
            };
            var existingQuestion = new Question
            {
                QuestionId = id,
                QuizId = 1,
                Text = "Old Question",
                OptionA = "A",
                OptionB = "B",
                OptionC = "C",
                OptionD = "D",
                CorrectAnswer = "A"
            };
            var updatedQuestion = new Question
            {
                QuestionId = id,
                QuizId = questionDTO.QuizId,
                Text = questionDTO.Text,
                OptionA = questionDTO.OptionA,
                OptionB = questionDTO.OptionB,
                OptionC = questionDTO.OptionC,
                OptionD = questionDTO.OptionD,
                CorrectAnswer = questionDTO.CorrectAnswer
            };
            _mockQuestionRepository.Setup(repo => repo.GetByKey(id)).ReturnsAsync(existingQuestion);
            _mockQuestionRepository.Setup(repo => repo.Update(It.IsAny<Question>())).ReturnsAsync(updatedQuestion);

            // Act
            var result = await _questionService.UpdateQuestion(id, questionDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(questionDTO.Text, result.Text);
            Assert.AreEqual(questionDTO.CorrectAnswer, result.CorrectAnswer);
        }

        [Test]
        public void UpdateQuestion_ShouldThrowNoSuchQuestionFoundException_WhenQuestionDoesNotExist()
        {
            // Arrange
            var id = 1;
            var questionDTO = new QuestionDTO
            {
                QuizId = 1,
                Text = "Updated Question"
            };
            _mockQuestionRepository.Setup(repo => repo.GetByKey(id)).ReturnsAsync((Question)null);

            // Act & Assert
            Assert.ThrowsAsync<NoSuchQuestionFoundException>(async () => await _questionService.UpdateQuestion(id, questionDTO));
        }

        [Test]
        public void UpdateQuestion_ShouldThrowQuestionUpdateFailedException_WhenExceptionOccurs()
        {
            // Arrange
            var id = 1;
            var questionDTO = new QuestionDTO
            {
                QuizId = 1,
                Text = "Updated Question"
            };
            _mockQuestionRepository.Setup(repo => repo.GetByKey(id)).ReturnsAsync(new Question { QuestionId = id });
            _mockQuestionRepository.Setup(repo => repo.Update(It.IsAny<Question>())).ThrowsAsync(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<QuestionUpdateFailedException>(async () => await _questionService.UpdateQuestion(id, questionDTO));
        }

        [Test]
        public async Task DeleteQuestion_ShouldReturnDeletedQuestion_WhenQuestionExists()
        {
            // Arrange
            var id = 1;
            var question = new Question
            {
                QuestionId = id,
                QuizId = 1,
                Text = "Sample Question"
            };
            _mockQuestionRepository.Setup(repo => repo.DeleteByKey(id)).ReturnsAsync(question);

            // Act
            var result = await _questionService.DeleteQuestion(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.QuestionId);
        }

        [Test]
        public void DeleteQuestion_ShouldThrowQuestionDeleteFailedException_WhenExceptionOccurs()
        {
            // Arrange
            var id = 1;
            _mockQuestionRepository.Setup(repo => repo.DeleteByKey(id)).ThrowsAsync(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<QuestionDeleteFailedException>(async () => await _questionService.DeleteQuestion(id));
        }

        [Test]
        public async Task GetAllQuestions_ShouldReturnQuestions_WhenQuestionsExist()
        {
            // Arrange
            var quizId = 1;
            var questions = new List<Question>
            {
                new Question { QuestionId = 1, QuizId = quizId, Text = "Question 1" },
                new Question { QuestionId = 2, QuizId = quizId, Text = "Question 2" }
            };
            _mockQuestionRepository.Setup(repo => repo.Get()).ReturnsAsync(questions);

            // Act
            var result = await _questionService.GetAllQuestions(quizId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(q => q.QuizId == quizId));
        }

        [Test]
        public void GetAllQuestions_ShouldThrowQuestionRetrievalFailedException_WhenExceptionOccurs()
        {
            // Arrange
            var quizId = 1;
            _mockQuestionRepository.Setup(repo => repo.Get()).ThrowsAsync(new Exception());

            // Act & Assert
            Assert.ThrowsAsync<QuestionRetrievalFailedException>(async () => await _questionService.GetAllQuestions(quizId));
        }
    }
}
