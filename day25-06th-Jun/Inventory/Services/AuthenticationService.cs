
using Inventory.Interfaces;
using Inventory.Models;
using Inventory.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace Inventory.Services
{
    public class AuthenticationService : IAutheticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptService _encryptionService;
        private readonly IUserService userService;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ITokenService tokenService,
                                    IEncryptService encryptionService,
                                    IUserService u,
                                    ILogger<AuthenticationService> logger)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            userService = u;
            _logger = logger;
        }
        public async Task<UserLoginResponse> Login(UserLoginRequest user)
        {
            var dbUser = await userService.GetByMail(user.Email);
            if (dbUser == null)
            {
                _logger.LogCritical("User not found");
                throw new Exception("No such user");
            }
            var encryptedData = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = user.Password,
            });
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, dbUser.Password);
            if (!isPasswordValid)
            {
                _logger.LogError("Invalid login attempt");
                throw new Exception("Invalid password");
            }
            var token = await _tokenService.TokenGenerator(dbUser);
            return new UserLoginResponse
            {
                Email = user.Email,
                Token = token,
            };
        }
    }
}