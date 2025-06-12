
using Inventory.Models;
namespace Inventory.Interfaces
{
    public interface IManagerService
    {
        Task<Manager> GetByMail(string mail);

        Task<CategoryAddRequest> RaiseAddCategoryRequest(string category);
    }
}