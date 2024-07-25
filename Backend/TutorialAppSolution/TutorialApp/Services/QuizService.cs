using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Quiz;

namespace TutorialApp.Services
{
    public class QuizService : IQuizService
    {
        private readonly IRepository<int, Quiz> _quizRepository;

        public QuizService(IRepository<int, Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<Quiz> GetQuizByCourseIdAsync(int courseId)
        {
            var quizzes = await _quizRepository.Get();
            var quiz = quizzes.FirstOrDefault(q => q.CourseId == courseId);
            if (quiz != null)
            {
                // Ensure questions are loaded
                quiz.Questions = quiz.Questions ?? new List<Question>();
            }
            return quiz;
        }

        public async Task<Quiz> CreateQuizAsync(QuizDTO quizDTO)
        {
            var quiz = new Quiz
            {
                CourseId = quizDTO.CourseId,
                Title = quizDTO.Title,
            };
            return await _quizRepository.Add(quiz);
        }

        public async Task<Quiz> DeleteQuizAsync(int quizId)
        {
            return await _quizRepository.DeleteByKey(quizId);
        }

    }
}
