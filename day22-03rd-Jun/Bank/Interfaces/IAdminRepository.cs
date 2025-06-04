using Bank.Models;
using Bank.Models.DTOs;
namespace Bank.Interfaces
{
    public interface IAdminRepository
    {
        public Task<IEnumerable<Admin>> GetAllAdmins();
        public Task<Admin> GetAdminById(string id);

        public Task<string> AddAdmin(Admin admin);
        public Task<Admin> GetAdminByMail(string email);
    }
}