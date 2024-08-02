using TutorialApp.Exceptions.Question;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.Question;
using TutorialApp.Models;

namespace TutorialApp.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<int, Question> _questionRepository;
      
        private readonly ILogger<QuestionService> _logger;

        public QuestionService(IRepository<int, Question> questionRepository, ILogger<QuestionService> logger)
        {
            _questionRepository = questionRepository;
         
            _logger = logger;
        }

        public async Task<Question> AddQuestion(QuestionDTO questionDto)
        {
            try
            {
                var question = new Question
                {
                    QuizId = questionDto.QuizId,
                    Text = questionDto.Text,
                    OptionA = questionDto.OptionA,
                    OptionB = questionDto.OptionB,
                    OptionC = questionDto.OptionC,
                    OptionD = questionDto.OptionD,
                    CorrectAnswer = questionDto.CorrectAnswer
                };

                var addedQuestion = await _questionRepository.Add(question);

                return addedQuestion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the question.");
                throw new CreateQuestionFailedException("An error occurred while adding the question.");
            }
        }

        public async Task<Question> UpdateQuestion(int id, QuestionDTO questionDto)
        {
            try
            {
                var question = await _questionRepository.GetByKey(id);
                if (question == null)
                {
                    throw new NoSuchQuestionFoundException();
                }

                question.QuizId = questionDto.QuizId;
                question.Text = questionDto.Text;
                question.OptionA = questionDto.OptionA;
                question.OptionB = questionDto.OptionB;
                question.OptionC = questionDto.OptionC;
                question.OptionD = questionDto.OptionD;
                question.CorrectAnswer = questionDto.CorrectAnswer;

                var updatedQuestion = await _questionRepository.Update(question);

                return updatedQuestion;
            }
            catch (NoSuchQuestionFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the question.");
                throw new QuestionUpdateFailedException("An error occurred while updating the question.", ex);
            }
        }

        public async Task<Question> DeleteQuestion(int id)
        {
            try
            {
                var deletedQuestion = await _questionRepository.DeleteByKey(id);
                return deletedQuestion;
            }
            catch (NoSuchQuestionFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the question.");
                throw new QuestionDeleteFailedException("An error occurred while deleting the question.");
            }
        }

        public async Task<IEnumerable<Question>> GetAllQuestions(int quizId)
        {
            try
            {
                var questions = await _questionRepository.Get();
               

                return questions.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving questions.");
                throw new QuestionRetrievalFailedException("An error occurred while retrieving questions.");
            }
        }

        
    }
}
