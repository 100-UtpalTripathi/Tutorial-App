using TutorialApp.Models;
using TutorialApp.Models.DTOs.Question;

namespace TutorialApp.Interfaces
{
    public interface IQuestionService
    {
        Task<Question> AddQuestion(QuestionDTO questionDto);
        Task<Question> UpdateQuestion(int id, QuestionDTO questionDto);
        Task<Question> DeleteQuestion(int id);
        Task<IEnumerable<Question>> GetAllQuestions(int quizId); // Updated method signature
        
    }
}
