using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TutorialApp.Exceptions.Quiz;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs;
using TutorialApp.Models.DTOs.Quiz;

namespace TutorialApp.Controllers
{
    [Route("api/course/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        #region Dependency Injection
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        #endregion

        #region Get Quiz By Course Id

        [HttpGet("get/{courseId}")]
        public async Task<IActionResult> GetQuizByCourseId(int courseId)
        {
            try
            {
                var quiz = await _quizService.GetQuizByCourseIdAsync(courseId);
                if (quiz == null)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, "Quiz not found", null);
                    return NotFound(errorResponse);
                }
                var response = new ApiResponse<Quiz>((int)HttpStatusCode.OK, "Quiz retrieved successfully", quiz);
                return Ok(response);
            }
            catch (NoSuchQuizFoundException ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, ex.Message, null);
                return NotFound(errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        #endregion

        #region Create Quiz

        [HttpPost("create")]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizDTO quiz)
        {
            try
            {
                var createdQuiz = await _quizService.CreateQuizAsync(quiz);
                if (createdQuiz == null)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, "Quiz creation failed", null);
                    return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
                }
                var response = new ApiResponse<Quiz>((int)HttpStatusCode.OK, "Quiz created successfully", createdQuiz);
                return Ok(response);
            }
            catch (QuizCreationFailedException ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        #endregion

        #region delete quiz

        [HttpDelete("delete/{quizId}")]
        public async Task<IActionResult> DeleteQuiz(int quizId)
        {
            try
            {
                var deletedQuiz = await _quizService.DeleteQuizAsync(quizId);
                if (deletedQuiz == null)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, "Quiz not found", null);
                    return NotFound(errorResponse);
                }
                var response = new ApiResponse<Quiz>((int)HttpStatusCode.OK, "Quiz deleted successfully", deletedQuiz);
                return Ok(response);
            }
            catch (NoSuchQuizFoundException ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, ex.Message, null);
                return NotFound(errorResponse);
            }
            catch (QuizDeletionFailedException ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        #endregion
    }
}
