using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs;
using TutorialApp.Models.DTOs.Cart;
using TutorialApp.Exceptions.Cart;
using TutorialApp.Exceptions.Course;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace TutorialApp.Controllers
{
    [Route("api/user/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class CartController : ControllerBase
    {
        #region Dependency Injection
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;


        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;   
        }

        #endregion

       
        #region Add To Cart
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartItem = await _cartService.AddToCartAsync(cartDTO);
                var response = new ApiResponse<Cart>((int)HttpStatusCode.OK, "Item added to cart successfully", cartItem);
                return Ok(response);
            }
            catch (DuplicateItemAddingException ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null);
                return Ok(errorResponse);
            }
            catch (NoSuchCourseFoundException ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null);
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        #endregion

       #region Remove From Cart
        [HttpDelete("delete")]
        public async Task<IActionResult> RemoveFromCart([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartItem = await _cartService.RemoveFromCartAsync(cartDTO);
                var response = new ApiResponse<Cart>((int)HttpStatusCode.OK, "Item removed from cart successfully", cartItem);
                return Ok(response);
            }
            catch (NoSuchCartFoundException ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null);
                return BadRequest(errorResponse);
            }
            catch (CartDeleteFailedException ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.BadRequest, ex.Message, null);
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        #endregion

        #region Get Cart Items By User

        [HttpGet("get/{userEmail}")]
        public async Task<IActionResult> GetCartItemsByUser(string userEmail)
        {
            try
            {
                var courses = await _cartService.GetCartItemsByUserAsync(userEmail);
                if (courses == null || !courses.Any())
                {
                    var notFoundResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, "No items found in cart", null);
                    return NotFound(notFoundResponse);
                }
                var response = new ApiResponse<IEnumerable<Course>>((int)HttpStatusCode.OK, "Cart items retrieved successfully", courses);
                return Ok(response);
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
