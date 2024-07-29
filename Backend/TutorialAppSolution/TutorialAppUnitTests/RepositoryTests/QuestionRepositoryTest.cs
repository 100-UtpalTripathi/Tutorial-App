using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Question;
using TutorialApp.Models;
using TutorialApp.Repositories;

namespace TutorialAppUnitTests.RepositoryTests
{
    public class QuestionRepositoryTest
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
        public async Task AddQuestionAsync_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuestionRepository(context);
                var question = new Question
                {
                    QuizId = 1,
                    Text = "What is 2 + 2?",
                    OptionA = "3",
                    OptionB = "4",
                    OptionC = "5",
                    OptionD = "6",
                    CorrectAnswer = "OptionB"
                };

                // Act
                var addedQuestion = await repository.Add(question);

                // Assert
                Assert.IsNotNull(addedQuestion);
                Assert.AreEqual(question.Text, addedQuestion.Text);
            }
        }

        [Test]
        public async Task DeleteQuestionByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuestionRepository(context);
                var question = new Question
                {
                    QuizId = 1,
                    Text = "What is 2 + 2?",
                    OptionA = "3",
                    OptionB = "4",
                    OptionC = "5",
                    OptionD = "6",
                    CorrectAnswer = "OptionB"
                };

                var addedQuestion = await repository.Add(question);

                // Act
                var deletedQuestion = await repository.DeleteByKey(addedQuestion.QuestionId);

                // Assert
                Assert.IsNotNull(deletedQuestion);
                Assert.AreEqual(addedQuestion.QuestionId, deletedQuestion.QuestionId);
            }
        }

        [Test]
        public void DeleteQuestionByKey_QuestionNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuestionRepository(context);

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchQuestionFoundException>(() => repository.DeleteByKey(100));
                Assert.AreEqual("No such question found", ex.Message);
            }
        }

        [Test]
        public async Task GetQuestionByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuestionRepository(context);
                var question = new Question
                {
                    QuizId = 1,
                    Text = "What is 2 + 2?",
                    OptionA = "3",
                    OptionB = "4",
                    OptionC = "5",
                    OptionD = "6",
                    CorrectAnswer = "OptionB"
                };

                var addedQuestion = await repository.Add(question);

                // Act
                var fetchedQuestion = await repository.GetByKey(addedQuestion.QuestionId);

                // Assert
                Assert.IsNotNull(fetchedQuestion);
                Assert.AreEqual(addedQuestion.QuestionId, fetchedQuestion.QuestionId);
            }
        }

        [Test]
        public async Task GetQuestionByKey_QuestionNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuestionRepository(context);

                // Act
                var question = await repository.GetByKey(100); // Assuming ID 100 does not exist

                // Assert
                Assert.IsNull(question);
            }
        }

        [Test]
        public async Task GetQuestions_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuestionRepository(context);
                var question1 = new Question
                {
                    QuizId = 1,
                    Text = "What is 2 + 2?",
                    OptionA = "3",
                    OptionB = "4",
                    OptionC = "5",
                    OptionD = "6",
                    CorrectAnswer = "OptionB"
                };
                var question2 = new Question
                {
                    QuizId = 2,
                    Text = "What is the capital of France?",
                    OptionA = "Berlin",
                    OptionB = "London",
                    OptionC = "Paris",
                    OptionD = "Rome",
                    CorrectAnswer = "OptionC"
                };

                await repository.Add(question1);
                await repository.Add(question2);

                // Act
                var questions = await repository.Get();

                // Assert
                Assert.IsNotNull(questions);
                Assert.AreEqual(2, questions.Count());
            }
        }

        [Test]
        public async Task UpdateQuestion_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuestionRepository(context);
                var question = new Question
                {
                    QuizId = 1,
                    Text = "What is 2 + 2?",
                    OptionA = "3",
                    OptionB = "4",
                    OptionC = "5",
                    OptionD = "6",
                    CorrectAnswer = "OptionB"
                };

                var addedQuestion = await repository.Add(question);

                addedQuestion.Text = "What is 3 + 3?";
                addedQuestion.OptionA = "5";
                addedQuestion.OptionB = "6";
                addedQuestion.OptionC = "7";
                addedQuestion.OptionD = "8";
                addedQuestion.CorrectAnswer = "OptionB";

                // Act
                var updatedQuestion = await repository.Update(addedQuestion);

                // Assert
                Assert.IsNotNull(updatedQuestion);
                Assert.AreEqual("What is 3 + 3?", updatedQuestion.Text);
            }
        }

        [Test]
        public void UpdateQuestion_QuestionNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuestionRepository(context);
                var invalidQuestion = new Question
                {
                    QuestionId = 100,
                    QuizId = 1,
                    Text = "What is 2 + 2?",
                    OptionA = "3",
                    OptionB = "4",
                    OptionC = "5",
                    OptionD = "6",
                    CorrectAnswer = "OptionB"
                };

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchQuestionFoundException>(() => repository.Update(invalidQuestion));
                Assert.AreEqual("No such question found", ex.Message);
            }
        }
    }
}
