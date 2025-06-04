
using Bank.Interfaces;
using Bank.Models;
using Bank.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace Bank.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenServices _tokenService;
        private readonly IEncryptService _encryptionService;
        private readonly IMasterRepository masterRepository;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ITokenServices tokenService,
                                    IEncryptService encryptionService,
                                    IMasterRepository m,
                                    ILogger<AuthenticationService> logger)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            masterRepository = m;
            _logger = logger;
        }
        public async Task<UserLoginResponse> Login(UserLoginRequest user)
        {
            var dbUser = await masterRepository.Get(user.Username);
            if (dbUser == null)
            {
                _logger.LogCritical("User not found");
                throw new Exception("No such user");
            }
            var encryptedData = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = user.Password,
                HashKey = dbUser.HashKey
            });
            for (int i = 0; i < encryptedData.EncryptedData.Length; i++)
            {
                if (encryptedData.EncryptedData[i] != dbUser.Password[i])
                {
                    _logger.LogError("Invalid login attempt");
                    throw new Exception("Invalid password");
                }
            }
            var token = await _tokenService.GenerateToken(dbUser);
            return new UserLoginResponse
            {
                UserName = user.Username,
                Token = token,
            };
        }
    }
}