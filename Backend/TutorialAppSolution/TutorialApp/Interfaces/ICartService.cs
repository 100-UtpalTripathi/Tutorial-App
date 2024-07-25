using TutorialApp.Models.DTOs.Cart;
using TutorialApp.Models;

namespace TutorialApp.Interfaces
{
    public interface ICartService
    {
        Task<Cart> AddToCartAsync(CartDTO cartDTO);
        Task<Cart> RemoveFromCartAsync(CartDTO cartDTO);
        Task<IEnumerable<Course>> GetCartItemsByUserAsync(string userEmail);
    }
}
