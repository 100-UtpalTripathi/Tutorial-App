using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using TutorialApp.Exceptions.User;
using TutorialApp.Exceptions.UserCredential;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.User;

namespace TutorialApp.Services
{
    public class UserService : IUserService
    {
        #region Dependency Injection
        private readonly IRepository<string, User> _userRepository;
        private readonly IRepository<int, Course> _courseRepository;
        private readonly IRepository<int, Category> _categoryRepository;
        private readonly IRepository<string, UserCredential> _userCredentialRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IRepository<string, User> userRepository, IRepository<int, Course> courseRepository, IRepository<int, Category> categoryRepository, IRepository<string, UserCredential> userCredentialRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _courseRepository = courseRepository;
            _categoryRepository = categoryRepository;
            _userCredentialRepository = userCredentialRepository;
            _logger = logger;
        }

        #endregion


        #region View Profile
        public async Task<UserProfileDTO> ViewProfileAsync(string userEmail)
        {
            var user = await _userRepository.GetByKey(userEmail);
            if (user == null)
            {
                throw new NoSuchUserFoundException();
            }

            return new UserProfileDTO
            {
                Email = user.Email,
                Name = user.Name,
                Dob = user.Dob,
                Phone = user.Phone,
                ImageUri = user.ImageURI,
                Password = "********",

            };
        }

        #endregion


        #region Edit Profile
        public async Task<UserProfileDTO> EditProfileAsync(UserProfileDTO userProfileDTO)
        {
            var user = await _userRepository.GetByKey(userProfileDTO.Email);
            if (user == null)
            {
                throw new NoSuchUserFoundException();
            }
            var userCredential = await _userCredentialRepository.GetByKey(userProfileDTO.Email);
            if (userCredential == null)
            {
                throw new NoSuchUserCredentialFoundException();
            }


            user.Name = userProfileDTO.Name;
            user.Phone = userProfileDTO.Phone;
            user.ImageURI = userProfileDTO.ImageUri;
            user.Dob = userProfileDTO.Dob;


            HMACSHA512 hMACSHA = new HMACSHA512();
            userCredential.PasswordHashKey = hMACSHA.Key;
            userCredential.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(userProfileDTO.Password));
            try
            {
                await _userRepository.Update(user);
                await _userCredentialRepository.Update(userCredential);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating user profile");
                throw new UserProfileUpdateFailedException("Error while updating user profile");
            }
            
            return userProfileDTO;
        }

        #endregion

        #region Search Courses By Category

        public async Task<IEnumerable<Course>> SearchCoursesByCategoryAsync(string category)
        {
            var existingCategories = await _categoryRepository.Get();

            var existingCategory = existingCategories.FirstOrDefault(c => c.Name == category);

            var courses = await _courseRepository.Get();
            return courses.Where(c => c.CategoryId == existingCategory.CategoryId);
        }

        #endregion
    }
}
