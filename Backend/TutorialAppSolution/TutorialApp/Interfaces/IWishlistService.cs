using TutorialApp.Models;
using TutorialApp.Models.DTOs.Wishlist;

namespace TutorialApp.Interfaces
{
    public interface IWishlistService
    {
        Task<Wishlist> AddToWishlistAsync(WishListDTO wishlistDTO);
        Task<IEnumerable<Course>> GetWishlistedCoursesByUserAsync(string userEmail);
    }
}
