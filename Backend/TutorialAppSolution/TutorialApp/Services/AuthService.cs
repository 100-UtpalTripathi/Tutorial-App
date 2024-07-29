using System.Security.Cryptography;
using System.Text;
using TutorialApp.Exceptions.User;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Models.DTOs.User;

namespace TutorialApp.Services
{
    public class AuthService : IAuthService
    {
        #region Private Fields

        private readonly IRepository<string, User> _userRepo;
        private readonly IRepository<string, UserCredential> _userCredentialRepo;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IRepository<string, User> userRepo, IRepository<string, UserCredential> userCredentialRepo, ITokenService tokenService, ILogger<AuthService> logger)
        {
            _userRepo = userRepo;
            _userCredentialRepo = userCredentialRepo;
            _tokenService = tokenService;
            _logger = logger;
        }

        #endregion


        #region Login
        public async Task<UserLoginReturnDTO> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var userCredential = await _userCredentialRepo.GetByKey(userLoginDTO.Email);
            if (userCredential == null)
            {
                throw new NoSuchUserFoundException();
            }

            HMACSHA512 hMACSHA = new HMACSHA512(userCredential.PasswordHashKey);
            var encryptedPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(userLoginDTO.Password));
            bool isPasswordSame = ComparePassword(encryptedPass, userCredential.Password);

            if(isPasswordSame)
            {
                var user = await _userRepo.GetByKey(userLoginDTO.Email);
                if(userCredential.Status == "Active")
                {
                    UserLoginReturnDTO userLoginReturnDTO = new UserLoginReturnDTO
                    {
                        Token = _tokenService.GenerateToken(user),
                        Email = user.Email,
                        Role = user.Role
                    };

                    return userLoginReturnDTO;
                }
            }

            _logger.LogWarning("Invalid Email or Password");
            throw new UnauthorizedUserException("Invalid Email or Password");
        }

        #endregion

        #region Register

        public async Task<UserRegisterReturnDTO> RegisterAsync(UserRegisterDTO userRegisterDTO)
        {
            User user = null;
            UserCredential userCredential = null;

            try
            {
                user = new User
                {
                    Email = userRegisterDTO.Email,
                    Role = userRegisterDTO.Role,
                    Name = userRegisterDTO.Name,
                    Dob = userRegisterDTO.Dob,
                    Phone = userRegisterDTO.Phone,
                    ImageURI = userRegisterDTO.ImageUri
                };

                HMACSHA512 hMACSHA = new HMACSHA512();

                userCredential = new UserCredential
                {
                    Email = userRegisterDTO.Email,
                    PasswordHashKey = hMACSHA.Key,
                    Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDTO.Password)),
                    Status = "Active"
                };

                user = await _userRepo.Add(user);

                userCredential = await _userCredentialRepo.Add(userCredential);

                return new UserRegisterReturnDTO
                {
                    Email = user.Email,
                    Role = user.Role
                };
            }
            catch (Exception)
            {
            }

            if(user != null)
            {
                await _userRepo.DeleteByKey(user.Email);
            }

            if(userCredential != null && user == null)
            {
                await _userCredentialRepo.DeleteByKey(userCredential.Email);
            }
            _logger.LogError("Not able to register at this moment!");
            throw new UserRegistrationFailedException("Not able to register at this moment!");
        }

        #endregion

        #region Helper Methods

        public bool ComparePassword(byte[] encryptedPass, byte[] password)
        {
            if (encryptedPass.Length != password.Length)
            {
                return false;
            }

            for (int i = 0; i < encryptedPass.Length; i++)
            {
                if (encryptedPass[i] != password[i])
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
