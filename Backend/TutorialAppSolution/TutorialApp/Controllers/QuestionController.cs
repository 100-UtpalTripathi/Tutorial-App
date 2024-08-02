using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Exceptions.Question;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Question;

namespace TutorialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // GET: api/Questions
        [HttpGet("get/{quizId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Question>>>> GetQuestions(int quizId)
        {
            try
            {
                var questions = await _questionService.GetAllQuestions(quizId);
                return Ok(new ApiResponse<IEnumerable<Question>>(200, "Questions retrieved successfully", questions));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<Question>>(500, ex.Message, null));
            }
        }


        // POST: api/Questions
        [HttpPost("add")]
        
        public async Task<ActionResult<ApiResponse<Question>>> PostQuestion([FromBody] QuestionDTO questionDto)
        {
            try
            {
                var addedQuestion = await _questionService.AddQuestion(questionDto);
                return Ok(new ApiResponse<Question>(201, "Question created successfully", addedQuestion));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<QuestionDTO>(500, ex.Message, null));
            }
        }


        // PUT: api/Questions/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult<ApiResponse<Question>>> PutQuestion(int id, [FromBody] QuestionDTO questionDto)
        {
            try
            {
                var updatedQuestion = await _questionService.UpdateQuestion(id, questionDto);
                return Ok(new ApiResponse<Question>(200, "Question updated successfully", updatedQuestion));
            }
            catch (NoSuchQuestionFoundException)
            {
                return NotFound(new ApiResponse<Question>(404, "Question not found", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Question>(500, ex.Message, null));
            }
        }

        // DELETE: api/Questions/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<ApiResponse<Question>>> DeleteQuestion(int id)
        {
            try
            {
                var deletedQuestion = await _questionService.DeleteQuestion(id);
                return Ok(new ApiResponse<Question>(200, "Question deleted successfully", deletedQuestion));
            }
            catch (NoSuchQuestionFoundException)
            {
                return NotFound(new ApiResponse<Question>(404, "Question not found", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Question>(500, ex.Message, null));
            }
        }
    }
}
