using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.Wishlist;
using TutorialApp.Models;

namespace TutorialApp.Controllers
{
    [Route("api/user/course/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<Wishlist>> AddToWishlist([FromBody] WishListDTO wishlistDTO)
        {
            var createdWishlist = await _wishlistService.AddToWishlistAsync(wishlistDTO);
            return CreatedAtAction(nameof(GetWishlistedCoursesByUser), new { userEmail = createdWishlist.UserEmail }, createdWishlist);
        }

        [HttpGet("get/{userEmail}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetWishlistedCoursesByUser(string userEmail)
        {
            var courses = await _wishlistService.GetWishlistedCoursesByUserAsync(userEmail);
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }
            return Ok(courses);
        }

        [HttpDelete("remove")]
        public async Task<ActionResult<Wishlist>> RemoveFromWishlist([FromBody] WishListDTO wishlistDTO)
        {
            var removedWishlist = await _wishlistService.RemoveFromWishlistAsync(wishlistDTO);
            if (removedWishlist == null)
            {
                return NotFound();
            }
            return Ok(removedWishlist);
        }
    }
}
