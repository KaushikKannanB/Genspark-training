
using MainMigration.Interfaces;
using MainMigration.Models;
using MainMigration.Models.DTOs;
using MainMigration.Repositories;

namespace MainMigration.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService userService;

        private readonly IRepository<int, User> userrepo;
        private readonly ILogger<AuthenticationService> _logger;
        public AuthenticationService(ITokenService tokenService,
                                    IUserService u, 
                                    ILogger<AuthenticationService> logger,IRepository<int, User> us)
        {
            _tokenService = tokenService;
            userService = u;
            _logger = logger;
            userrepo = us;
        }

        public async Task<User> SignUp(UserSignUpRequest request)
        {
            var exist_user = await userService.GetByUserName(request.UserName);

            if (exist_user != null)
            {
                return null;
            }
            else
            {
                User u = new();
                u.Username = request.UserName.ToLower();
                u.Password = request.Password;
                u.CustomerAddress = request.CustomerAddress;
                u.CustomerEmail = request.CustomerEmail;
                u.CustomerPhone = request.CustomerPhone;
                var user = await userrepo.Add(u);
                return user;
            }
        }
        public async Task<UserLoginResponse> Login(UserLoginRequest user)
        {
            var dbUser = await userService.GetByUserName(user.UserName);
            if (dbUser == null)
            {
                _logger.LogCritical("User not found");
                throw new Exception("No such user");
            }

            bool isPasswordValid = user.Password == dbUser.Password;
            if (!isPasswordValid)
            {
                _logger.LogError("Invalid login attempt");
                throw new Exception("Invalid password");
            }

            var accessToken = await _tokenService.TokenGenerator(dbUser);


            return new UserLoginResponse
            {
                UserName = user.UserName,
                Token = accessToken,
            };
        }

        

    }
}