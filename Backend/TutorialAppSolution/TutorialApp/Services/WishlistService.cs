using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.Wishlist;
using TutorialApp.Models;

namespace TutorialApp.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IRepository<int, Wishlist> _wishlistRepository;
        private readonly IRepository<int, Course> _courseRepository;

        public WishlistService(IRepository<int, Wishlist> wishlistRepository, IRepository<int, Course> courseRepository)
        {
            _wishlistRepository = wishlistRepository;
            _courseRepository = courseRepository;
        }

        public async Task<Wishlist> AddToWishlistAsync(WishListDTO wishlistDTO)
        {
            var wishlist = new Wishlist
            {
                UserEmail = wishlistDTO.UserEmail,
                CourseId = wishlistDTO.CourseId
            };
            return await _wishlistRepository.Add(wishlist);
        }

        public async Task<IEnumerable<Course>> GetWishlistedCoursesByUserAsync(string userEmail)
        {
            var wishlists = await _wishlistRepository.Get();
            var userWishlists = wishlists.Where(w => w.UserEmail == userEmail);
            var courses = new List<Course>();

            foreach (var wishlist in userWishlists)
            {
                var course = await _courseRepository.GetByKey(wishlist.CourseId);
                if (course != null)
                {
                    courses.Add(course);
                }
            }

            return courses;
        }
    }
}
