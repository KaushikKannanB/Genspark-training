using Inventory.Models;
using Inventory.Models.DTOs;

namespace Inventory.Misc
{
    public class ManagerMapper
    {
        public Manager? MapManager(AdminManagerAddRequestDTO request)
        {
            Manager m = new();
            m.Name = request.Name;
            m.Email = request.Email;
            m.Status = "ACTIVE";

            return m;
        }
    }
}