using Inventory.Interfaces;
using Inventory.Contexts;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;


namespace Inventory.Services
{
    public class ManagerService : IManagerService
    {
        private readonly InventoryContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IUserService userService;
        private readonly IEncryptService encryptService;



        private readonly IRepository<string, Category> categrepo;
        private readonly IRepository<string, CategoryAddRequest> categaddrepo;


        public ManagerService(IRepository<string, CategoryAddRequest> cat, IEncryptService en, IUserService us, ICurrentUserService cu, InventoryContext c, IRepository<string, Category> ca)
        {
            context = c;
            categrepo = ca;
            currentUserService = cu;
            userService = us;
            encryptService = en;
            categaddrepo = cat;
        }

        public async Task<Manager> GetByMail(string mail)
        {
            var manager = await context.Managers.FirstOrDefaultAsync(a => a.Email == mail);
            return manager;
        }

        public async Task<CategoryAddRequest> RaiseAddCategoryRequest(string category)
        {
            category = category.ToUpper().Trim();

            var catexists = await categrepo.GetByName(category);
            var categaddexists = await categaddrepo.GetByName(category);
            if (catexists != null)
            {
                return null;

            }
            if (categaddexists != null)
            {
                if (categaddexists.Status != "DENIED")
                {
                    return null;
                }
            }
            if (catexists == null)
                {
                    var ca_id = await encryptService.GenerateId("CATADD");
                    var cur_user = await userService.GetByMail(currentUserService.Email);
                    CategoryAddRequest ca = new();
                    ca.Id = ca_id;
                    ca.CategoryName = category;
                    ca.RequestedAt = DateTime.UtcNow;
                    ca.RequestedBy = cur_user.Id;
                    ca.Status = "REQUESTED";

                    var ca_added = await categaddrepo.Add(ca);

                    return ca_added;


                }
                else
                {
                    return null;
                }
        }
    }
}