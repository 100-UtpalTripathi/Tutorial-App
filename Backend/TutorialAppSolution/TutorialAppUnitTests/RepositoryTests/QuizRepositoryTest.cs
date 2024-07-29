using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Quiz;
using TutorialApp.Models;
using TutorialApp.Repositories;

namespace TutorialAppUnitTests.RepositoryTests
{
    public class QuizRepositoryTest
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
        public async Task AddQuizAsync_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuizRepository(context);
                var quiz = new Quiz
                {
                    CourseId = 1,
                    Title = "Sample Quiz",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "What is 2 + 2?",
                            OptionA = "3",
                            OptionB = "4",
                            OptionC = "5",
                            OptionD = "6",
                            CorrectAnswer = "OptionB"
                        }
                    }
                };

                // Act
                var addedQuiz = await repository.Add(quiz);

                // Assert
                Assert.IsNotNull(addedQuiz);
                Assert.AreEqual(quiz.Title, addedQuiz.Title);
            }
        }

        [Test]
        public async Task DeleteQuizByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuizRepository(context);
                var quiz = new Quiz
                {
                    CourseId = 1,
                    Title = "Sample Quiz",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "What is 2 + 2?",
                            OptionA = "3",
                            OptionB = "4",
                            OptionC = "5",
                            OptionD = "6",
                            CorrectAnswer = "OptionB"
                        }
                    }
                };

                var addedQuiz = await repository.Add(quiz);

                // Act
                var deletedQuiz = await repository.DeleteByKey(addedQuiz.QuizId);

                // Assert
                Assert.IsNotNull(deletedQuiz);
                Assert.AreEqual(addedQuiz.QuizId, deletedQuiz.QuizId);
            }
        }

        [Test]
        public void DeleteQuizByKey_QuizNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuizRepository(context);

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchQuizFoundException>(() => repository.DeleteByKey(100));
                Assert.AreEqual("No such quiz found", ex.Message);
            }
        }

        [Test]
        public async Task GetQuizByKey_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuizRepository(context);
                var quiz = new Quiz
                {
                    CourseId = 1,
                    Title = "Sample Quiz",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "What is 2 + 2?",
                            OptionA = "3",
                            OptionB = "4",
                            OptionC = "5",
                            OptionD = "6",
                            CorrectAnswer = "OptionB"
                        }
                    }
                };

                var addedQuiz = await repository.Add(quiz);

                // Act
                var fetchedQuiz = await repository.GetByKey(addedQuiz.QuizId);

                // Assert
                Assert.IsNotNull(fetchedQuiz);
                Assert.AreEqual(addedQuiz.QuizId, fetchedQuiz.QuizId);
            }
        }

        [Test]
        public async Task GetQuizByKey_QuizNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuizRepository(context);

                // Act
                var quiz = await repository.GetByKey(100); // Assuming ID 100 does not exist

                // Assert
                Assert.IsNull(quiz);
            }
        }

        [Test]
        public async Task GetQuizzes_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuizRepository(context);
                var quiz1 = new Quiz
                {
                    CourseId = 1,
                    Title = "Sample Quiz 1",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "What is 2 + 2?",
                            OptionA = "3",
                            OptionB = "4",
                            OptionC = "5",
                            OptionD = "6",
                            CorrectAnswer = "OptionB"
                        }
                    }
                };
                var quiz2 = new Quiz
                {
                    CourseId = 2,
                    Title = "Sample Quiz 2",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "What is the capital of France?",
                            OptionA = "Berlin",
                            OptionB = "London",
                            OptionC = "Paris",
                            OptionD = "Rome",
                            CorrectAnswer = "OptionC"
                        }
                    }
                };

                await repository.Add(quiz1);
                await repository.Add(quiz2);

                // Act
                var quizzes = await repository.Get();

                // Assert
                Assert.IsNotNull(quizzes);
                Assert.AreEqual(2, quizzes.Count());
            }
        }

        [Test]
        public async Task UpdateQuiz_Success()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuizRepository(context);
                var quiz = new Quiz
                {
                    CourseId = 1,
                    Title = "Sample Quiz",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "What is 2 + 2?",
                            OptionA = "3",
                            OptionB = "4",
                            OptionC = "5",
                            OptionD = "6",
                            CorrectAnswer = "OptionB"
                        }
                    }
                };

                var addedQuiz = await repository.Add(quiz);

                addedQuiz.Title = "Updated Quiz Title";
                addedQuiz.Questions.First().Text = "What is 3 + 3?";

                // Act
                var updatedQuiz = await repository.Update(addedQuiz);

                // Assert
                Assert.IsNotNull(updatedQuiz);
                Assert.AreEqual("Updated Quiz Title", updatedQuiz.Title);
                Assert.AreEqual("What is 3 + 3?", updatedQuiz.Questions.First().Text);
            }
        }

        [Test]
        public void UpdateQuiz_QuizNotFound()
        {
            // Arrange
            using (var context = new TutorialAppContext(_options))
            {
                var repository = new QuizRepository(context);
                var invalidQuiz = new Quiz
                {
                    QuizId = 100,
                    CourseId = 1,
                    Title = "Non-existent Quiz",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "What is 2 + 2?",
                            OptionA = "3",
                            OptionB = "4",
                            OptionC = "5",
                            OptionD = "6",
                            CorrectAnswer = "OptionB"
                        }
                    }
                };

                // Act & Assert
                var ex = Assert.ThrowsAsync<NoSuchQuizFoundException>(() => repository.Update(invalidQuiz));
                Assert.AreEqual("No such quiz found", ex.Message);
            }
        }
    }
}
