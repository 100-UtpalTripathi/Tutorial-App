using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.Wishlist;
using TutorialApp.Models;

namespace TutorialApp.Services
{
    public class WishlistService : IWishlistService
    {

        #region Dependency Injection
        private readonly IRepository<int, Wishlist> _wishlistRepository;
        private readonly IRepository<int, Course> _courseRepository;

        public WishlistService(IRepository<int, Wishlist> wishlistRepository, IRepository<int, Course> courseRepository)
        {
            _wishlistRepository = wishlistRepository;
            _courseRepository = courseRepository;
        }

        #endregion

        #region Add To Wishlist

        public async Task<Wishlist> AddToWishlistAsync(WishListDTO wishlistDTO)
        {
            var wishlist = new Wishlist
            {
                UserEmail = wishlistDTO.UserEmail,
                CourseId = wishlistDTO.CourseId
            };
            return await _wishlistRepository.Add(wishlist);
        }

        #endregion

        #region Remove From Wishlist
        public async Task<Wishlist> RemoveFromWishlistAsync(WishListDTO wishlistDTO)
        {
            var wishlists = await _wishlistRepository.Get();
            var wishlist = wishlists.FirstOrDefault(w => w.UserEmail == wishlistDTO.UserEmail && w.CourseId == wishlistDTO.CourseId);

            if (wishlist == null)
            {
                throw new Exception("Item not found in wishlist.");
            }

            return await _wishlistRepository.DeleteByKey(wishlist.WishlistId);
        }
        #endregion

        #region Get Wishlisted Courses By User

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

        #endregion
    }
}
