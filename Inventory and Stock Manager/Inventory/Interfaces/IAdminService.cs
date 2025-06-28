using Inventory.Models.DTOs;
using Inventory.Models;
namespace Inventory.Interfaces
{
    public interface IAdminService
    {
        Task<Admin> GetByMail(string mail);
        Task<Admin> AddAdmin(AdminManagerAddRequestDTO request);
        Task<Manager> AddManager(AdminManagerAddRequestDTO request);
        Task<Category> AddCategory(string category);

        Task<Manager> DeleteManager(string ManagerId);
        Task<object> CheckManagerActivity(string ManagerId);
        Task<object> AdminActivity(string Id);

    }
}