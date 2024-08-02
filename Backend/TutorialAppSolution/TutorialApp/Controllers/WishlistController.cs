using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs;
using TutorialApp.Models.DTOs.Wishlist;
using TutorialApp.Models;
using TutorialApp.Exceptions.Wishlist;
using System.Net;
using Microsoft.AspNetCore.Cors;

namespace TutorialApp.Controllers
{
    [Route("api/user/course/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    
    public class WishlistController : ControllerBase
    {
        #region Dependency Injection
        private readonly IWishlistService _wishlistService;
        private readonly ILogger<WishlistController> _logger;
        public WishlistController(IWishlistService wishlistService, ILogger<WishlistController> logger)
        {
            _wishlistService = wishlistService;
            _logger = logger;
        }
        #endregion

        #region Add to Wishlist

        [HttpPost("add")]
        public async Task<IActionResult> AddToWishlist([FromBody] WishListDTO wishlistDTO)
        {
            try
            {
                var createdWishlist = await _wishlistService.AddToWishlistAsync(wishlistDTO);
                if (createdWishlist == null)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, "Unable to add to wishlist", null);
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }
                var response = new ApiResponse<Wishlist>((int)HttpStatusCode.OK, "Added to wishlist successfully", createdWishlist);
                return Ok(response);
            }
            catch (UnableToAddToWishlistException ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        #endregion

        #region Get Wishlisted Courses

        [HttpGet("get/{userEmail}")]
        public async Task<IActionResult> GetWishlistedCoursesByUser(string userEmail)
        {
            try
            {
                var courses = await _wishlistService.GetWishlistedCoursesByUserAsync(userEmail);
                if (courses == null || !courses.Any())
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, "No wishlisted courses found", null);
                    return Ok(errorResponse);
                }
                var response = new ApiResponse<IEnumerable<Course>>((int)HttpStatusCode.OK, "Wishlisted courses retrieved successfully", courses);
                return Ok(response);
            }
            catch (NoSuchWishlistFoundException ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, ex.Message, null);
                return Ok(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        #endregion

        #region Remove from Wishlist

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromWishlist([FromBody] WishListDTO wishlistDTO)
        {
            try
            {
                var removedWishlist = await _wishlistService.RemoveFromWishlistAsync(wishlistDTO);
                if (removedWishlist == null)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.NotFound, "Wishlist not found", null);
                    return NotFound(errorResponse);
                }
                var response = new ApiResponse<Wishlist>((int)HttpStatusCode.OK, "Removed from wishlist successfully", removedWishlist);
                return Ok(response);
            }
            catch (NoSuchWishlistFoundException ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            catch (UnableToRemoveFromWishlistException ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var errorResponse = new ApiResponse<string>((int)HttpStatusCode.InternalServerError, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        #endregion
    }
}
