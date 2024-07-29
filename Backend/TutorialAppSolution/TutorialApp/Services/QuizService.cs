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

        public QuizService(IRepository<int, Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }

        #endregion


        #region Get Quiz By Course Id

        public async Task<Quiz> GetQuizByCourseIdAsync(int courseId)
        {
            var quizzes = await _quizRepository.Get();
            if(quizzes == null)
            {
                throw new NoSuchQuizFoundException();
            }

            var quiz = quizzes.FirstOrDefault(q => q.CourseId == courseId);
            if (quiz != null)
            {
                // Ensure questions are loaded
                quiz.Questions = quiz.Questions ?? new List<Question>();
            }
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
                throw new QuizDeletionFailedException();
            }
            
        }

        #endregion

    }
}
