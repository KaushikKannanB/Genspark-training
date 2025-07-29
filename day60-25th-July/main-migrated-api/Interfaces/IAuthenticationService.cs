

using MainMigration.Models;
using MainMigration.Models.DTOs;

namespace MainMigration.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginResponse> Login(UserLoginRequest request);
        Task<User> SignUp(UserSignUpRequest request);
    }
}