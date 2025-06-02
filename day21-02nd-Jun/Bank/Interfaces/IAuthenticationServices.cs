using Bank.Models;
using Bank.Models.DTOs;
namespace Bank.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserLoginResponse> Login(UserLoginRequest request);
    }
}