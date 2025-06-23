using Notify.Models.DTO;

namespace Notify.Interfaces
{
    public interface IAutheticationService
    {
        Task<UserLoginResponse> Login(UserLoginRequest request);
    }
}