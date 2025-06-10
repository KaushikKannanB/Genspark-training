
using Inventory.Interfaces;
using Inventory.Models;
using Inventory.Models.DTOs;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace Inventory.Services
{
    public class AuthenticationService : IAutheticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptService _encryptionService;
        private readonly IUserService userService;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IBlacklistedTokenRepository _tokenBlacklist;

        public AuthenticationService(ITokenService tokenService,
                                    IEncryptService encryptionService,
                                    IUserService u,
                                    ILogger<AuthenticationService> logger, IBlacklistedTokenRepository _to)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            userService = u;
            _logger = logger;
            _tokenBlacklist = _to;
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
        public async Task<bool> Logout(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var expiry = jwtToken.ValidTo;

            var blacklistedToken = new BlacklistedToken
            {
                Token = token,
                ExpiryDate = expiry.ToUniversalTime()
            };

            await _tokenBlacklist.AddTokenAsync(blacklistedToken);
            _logger.LogInformation("Token blacklisted: {Token}", token);
            return true;
        }

    }
}