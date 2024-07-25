using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.Cart;
using TutorialApp.Models;

namespace TutorialApp.Controllers
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<Cart>> AddToCart([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartItem = await _cartService.AddToCartAsync(cartDTO);
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<Cart>> RemoveFromCart([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartItem = await _cartService.RemoveFromCartAsync(cartDTO);
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{userEmail}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCartItemsByUser(string userEmail)
        {
            var courses = await _cartService.GetCartItemsByUserAsync(userEmail);
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }
            return Ok(courses);
        }
    }
}
