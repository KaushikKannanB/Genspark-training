using Bank.Models;
using Bank.Models.DTOs;
namespace Bank.Interfaces
{
    public interface IAdminService
    {
        Task<string> AddAdmin(Admin admin);
        Task<IEnumerable<Admin>> GetAllAdmin();
        Task<Admin> GetAdminById(string id);
        Task<Admin> GetAdminByMail(string email);
    }
}

