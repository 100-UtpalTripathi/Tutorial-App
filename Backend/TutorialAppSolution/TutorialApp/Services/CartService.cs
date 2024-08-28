
using TutorialApp.Exceptions.Cart;
using TutorialApp.Exceptions.Course;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.Cart;

namespace TutorialApp.Services
{
    public class CartService : ICartService
    {

        #region Dependency Injection
        private readonly IRepository<int, Cart> _cartRepository;
        private readonly IRepository<int, Course> _courseRepository;
        private readonly IRepository<int, Enrollment> _enrollmentRepository;
        private readonly ILogger<CartService> _logger;

        public CartService(IRepository<int, Cart> cartRepository, IRepository<int, Course> courseRepository, IRepository<int, Enrollment> enrollmentRepository, ILogger<CartService> logger)
        {
            _cartRepository = cartRepository;
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
            _logger = logger;
        }

        #endregion


        #region Add To Cart
        public async Task<Cart> AddToCartAsync(CartDTO cartDTO)
        {
            var existingCartItems = await _cartRepository.Get();
            var existingCartItem = existingCartItems.FirstOrDefault(c => c.UserEmail == cartDTO.UserEmail && c.CourseId == cartDTO.CourseId);

            if (existingCartItem != null)
            {
                throw new DuplicateItemAddingException("Item already exists in cart.");
            }

            // Check if the user has completed 3 courses
            var existingEnrollments = await _enrollmentRepository.Get();
            var completedCoursesCount = existingEnrollments.Where(e => e.UserEmail == cartDTO.UserEmail && e.Status == "Completed")
                .Count();

            var course = await _courseRepository.GetByKey(cartDTO.CourseId);
            if (course == null)
            {
                throw new NoSuchCourseFoundException("Course not found.");
            }

            var price = course.Price;
            if (completedCoursesCount >= 3)
            {
                // Apply 10% discount
                price = price * 0.9m;
            }

            var cartItem = new Cart
            {
                UserEmail = cartDTO.UserEmail,
                CourseId = cartDTO.CourseId,
                Price = price
            };

            _logger.LogInformation($"Adding item to cart: {cartItem.UserEmail} - {cartItem.CourseId}");
            return await _cartRepository.Add(cartItem);
        }

        #endregion


        #region Remove From Cart

        public async Task<Cart> RemoveFromCartAsync(CartDTO cartDTO)
        {
            var existingCartItems = await _cartRepository.Get();
            var existingCartItem = existingCartItems.FirstOrDefault(c => c.UserEmail == cartDTO.UserEmail && c.CourseId == cartDTO.CourseId);

            if (existingCartItem == null)
            {
                throw new NoSuchCartFoundException("Item not found in cart.");
            }

            try
            {
                return await _cartRepository.DeleteByKey(existingCartItem.CartId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete item from cart.");
                throw new CartDeleteFailedException("Failed to delete item from cart.");
            }
        }

        #endregion

        #region Get Cart Items By User

        public async Task<IEnumerable<Course>> GetCartItemsByUserAsync(string userEmail)
        {
            var existingCartItems = await _cartRepository.Get();
            if (existingCartItems == null)
            {
                return new List<Course>();
            }

            var cartItems = existingCartItems.Where(c => c.UserEmail == userEmail).ToList(); // Get all cart items for the user

            var courseIds = cartItems.Select(c => c.CourseId).Distinct();
            var courses = new List<Course>();

            foreach (var courseId in courseIds)
            {
                var course = await _courseRepository.GetByKey(courseId);
                if (course != null)
                {
                    var cartItem = cartItems.FirstOrDefault(c => c.CourseId == courseId);
                    if (cartItem != null)
                    {
                        course.Price = cartItem.Price; // Update the course price with the price from the cart item
                    }
                    courses.Add(course);
                }
            }

            return courses;
        }


        #endregion
    }
}
