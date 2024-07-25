using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.Cart;
using TutorialApp.Models;
using Microsoft.EntityFrameworkCore;


namespace TutorialApp.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<int, Cart> _cartRepository;
        private readonly IRepository<int, Course> _courseRepository;

        public CartService(IRepository<int, Cart> cartRepository, IRepository<int, Course> courseRepository)
        {
            _cartRepository = cartRepository;
            _courseRepository = courseRepository;
        }

        public async Task<Cart> AddToCartAsync(CartDTO cartDTO)
        {
            var existingCartItems = await _cartRepository.Get();
            var existingCartItem = existingCartItems.FirstOrDefault(c => c.UserEmail == cartDTO.UserEmail && c.CourseId == cartDTO.CourseId);

            if (existingCartItem != null)
            {
                throw new Exception("Item already exists in cart.");
            }

            var cartItem = new Cart
            {
                UserEmail = cartDTO.UserEmail,
                CourseId = cartDTO.CourseId
            };
            return await _cartRepository.Add(cartItem);
        }

        public async Task<Cart> RemoveFromCartAsync(CartDTO cartDTO)
        {
            var existingCartItems = await _cartRepository.Get();
            var existingCartItem =  existingCartItems.FirstOrDefault(c => c.UserEmail == cartDTO.UserEmail && c.CourseId == cartDTO.CourseId);

            if (existingCartItem == null)
            {
                throw new Exception("Item not found in cart.");
            }

            return await _cartRepository.DeleteByKey(existingCartItem.CartId);
        }

        public async Task<IEnumerable<Course>> GetCartItemsByUserAsync(string userEmail)
        {
            var cartItems = await _cartRepository.Get();
            var userCartItems = cartItems.Where(c => c.UserEmail == userEmail);
            var courses = new List<Course>();

            foreach (var cartItem in userCartItems)
            {
                var course = await _courseRepository.GetByKey(cartItem.CourseId);
                if (course != null)
                {
                    courses.Add(course);
                }
            }

            return courses;
        }
    }
}
