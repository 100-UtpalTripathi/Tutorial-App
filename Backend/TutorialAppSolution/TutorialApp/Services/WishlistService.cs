using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.Wishlist;
using TutorialApp.Models;
using TutorialApp.Exceptions.Wishlist;

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

            try
            {
                return await _wishlistRepository.Add(wishlist);
            }
            catch (Exception ex)
            {
                throw new UnableToAddToWishlistException("Unable to add to wishlist.");
            }
            
        }

        #endregion

        #region Remove From Wishlist
        public async Task<Wishlist> RemoveFromWishlistAsync(WishListDTO wishlistDTO)
        {
            var wishlists = await _wishlistRepository.Get();

            if(wishlists == null || wishlists.Count() == 0)
            {
                throw new NoSuchWishlistFoundException("No wishlists found.");
            }
            var wishlist = wishlists.FirstOrDefault(w => w.UserEmail == wishlistDTO.UserEmail && w.CourseId == wishlistDTO.CourseId);

            if (wishlist == null)
            {
                throw new NoSuchWishlistFoundException("No wishlist found.");
            }

            try
            {
                return await _wishlistRepository.DeleteByKey(wishlist.WishlistId);
            }
            catch (Exception ex)
            {
                throw new UnableToRemoveFromWishlistException("Unable to remove from wishlist.");
            }
            
        }
        #endregion

        #region Get Wishlisted Courses By User

        public async Task<IEnumerable<Course>> GetWishlistedCoursesByUserAsync(string userEmail)
        {
            var wishlists = await _wishlistRepository.Get();

            if (wishlists == null || wishlists.Count() == 0)
            {
                throw new NoSuchWishlistFoundException("No wishlists found.");
            }
            var userWishlists = wishlists.Where(w => w.UserEmail == userEmail);

            if (userWishlists == null || userWishlists.Count() == 0)
            {
                throw new NoSuchWishlistFoundException("No wishlists found for the user.");
            }
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
