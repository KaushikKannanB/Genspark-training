using Inventory.Models.DTOs;

namespace Inventory.Interfaces
{
    public interface IAutheticationService
    {
        Task<UserLoginResponse> Login(UserLoginRequest request);
        Task<bool> Logout(string token);
    }
}