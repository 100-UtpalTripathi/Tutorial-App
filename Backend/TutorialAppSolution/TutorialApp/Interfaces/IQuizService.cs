using TutorialApp.Models;
using TutorialApp.Models.DTOs.Quiz;

namespace TutorialApp.Interfaces
{
    public interface IQuizService
    {
        Task<Quiz> GetQuizByCourseIdAsync(int courseId);
        Task<Quiz> CreateQuizAsync(QuizDTO quizDTO);

        Task<Quiz> DeleteQuizAsync(int quizId);
    }
}
