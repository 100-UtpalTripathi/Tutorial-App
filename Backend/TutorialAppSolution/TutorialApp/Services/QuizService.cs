using TutorialApp.Exceptions.Quiz;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Quiz;

namespace TutorialApp.Services
{
    public class QuizService : IQuizService
    {

        #region Dependency Injection
        private readonly IRepository<int, Quiz> _quizRepository;
        private readonly ILogger<QuizService> _logger;
        public QuizService(IRepository<int, Quiz> quizRepository, ILogger<QuizService> logger)
        {
            _quizRepository = quizRepository;
            _logger = logger;
        }

        #endregion


        #region Get Quiz By Course Id

        public async Task<Quiz> GetQuizByCourseIdAsync(int courseId)
        {
            var quizzes = await _quizRepository.Get();

            // Check if quizzes is null or empty
            if (quizzes == null || !quizzes.Any())
            {
                throw new NoSuchQuizFoundException("No quizzes found.");
            }

            // Find the quiz by courseId
            var quiz = quizzes.FirstOrDefault(q => q.CourseId == courseId);

            if (quiz == null)
            {
                throw new NoSuchQuizFoundException("No quiz found for the provided course ID.");
            }

            // Ensure questions are loaded
            quiz.Questions = quiz.Questions ?? new List<Question>();

            return quiz;
        }

        #endregion

        #region Create Quiz

        public async Task<Quiz> CreateQuizAsync(QuizDTO quizDTO)
        {
            var quiz = new Quiz
            {
                CourseId = quizDTO.CourseId,
                Title = quizDTO.Title,
            };

            try
            {
                return await _quizRepository.Add(quiz);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Quiz creation failed");
                throw new QuizCreationFailedException();
            }
            
        }

        #endregion

        #region Delete Quiz
        public async Task<Quiz> DeleteQuizAsync(int quizId)
        {
            try
            {
                return await _quizRepository.DeleteByKey(quizId);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Quiz deletion failed");
                throw new QuizDeletionFailedException();
            }
            
        }

        #endregion

    }
}
