
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
        private readonly IManagerService managerService;

        private readonly ILogger<AuthenticationService> _logger;
        private readonly IBlacklistedTokenRepository _tokenBlacklist;
        private readonly IRefreshTokenRepository _refreshTokenRepo;

        public AuthenticationService(ITokenService tokenService,
                                    IEncryptService encryptionService,
                                    IUserService u, IManagerService ma, IRefreshTokenRepository _ref,
                                    ILogger<AuthenticationService> logger, IBlacklistedTokenRepository _to)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            userService = u;
            _logger = logger;
            _tokenBlacklist = _to;
            managerService = ma;
            _refreshTokenRepo = _ref;
        }
        public async Task<UserLoginResponse> Login(UserLoginRequest user)
        {
            var dbUser = await userService.GetByMail(user.Email);
            if (dbUser == null)
            {
                _logger.LogCritical("User not found");
                throw new Exception("No such user");
            }

            if (dbUser.Role == "MANAGER")
            {
                var manager = await managerService.GetByMail(user.Email);
                if (manager.Status == "INACTIVE")
                {
                    _logger.LogCritical("User inactive");
                    throw new Exception("User is inactive i.e., deleted");
                }
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, dbUser.Password);
            if (!isPasswordValid)
            {
                _logger.LogError("Invalid login attempt");
                throw new Exception("Invalid password");
            }

            var accessToken = await _tokenService.TokenGenerator(dbUser);

            var refreshToken = await _tokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Email = dbUser.Email,
                ExpiryDate = DateTime.UtcNow.AddDays(7), 
                IsUsed = false,
                IsRevoked = false
            };
            await _refreshTokenRepo.SaveRefreshTokenAsync(refreshTokenEntity);

            
            return new UserLoginResponse
            {
                Email = user.Email,
                Token = accessToken,
                RefreshToken = refreshToken
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