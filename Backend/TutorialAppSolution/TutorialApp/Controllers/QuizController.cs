using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Quiz;

namespace TutorialApp.Controllers
{
    [Route("api/course/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet("get/{courseId}")]
        public async Task<ActionResult<Quiz>> GetQuizByCourseId(int courseId)
        {
            var quiz = await _quizService.GetQuizByCourseIdAsync(courseId);
            if (quiz == null)
            {
                return NotFound();
            }
            return Ok(quiz);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Quiz>> CreateQuiz([FromBody] QuizDTO quiz)
        {
            var createdQuiz = await _quizService.CreateQuizAsync(quiz);
            if(createdQuiz == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(createdQuiz);
        }

        [HttpDelete("delete/{quizId}")]
        public async Task<ActionResult<Quiz>> DeleteQuiz(int quizId)
        {
            var deletedQuiz = await _quizService.DeleteQuizAsync(quizId);
            if (deletedQuiz == null)
            {
                return NotFound();
            }
            return Ok(deletedQuiz);
        }
    }
}
