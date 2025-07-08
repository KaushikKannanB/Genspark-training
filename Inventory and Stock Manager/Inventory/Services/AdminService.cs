using Inventory.Contexts;
using Inventory.Interfaces;
using Inventory.Misc;
using Inventory.Models;
using Inventory.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Nodes;

namespace Inventory.Services
{
    public class AdminService : IAdminService
    {
        ManagerMapper managerMapper;
        private readonly InventoryContext context;
        private readonly IManagerService managerService;
        private readonly ICurrentUserService currentUserService;

        private readonly IRepository<string, Admin> adminrepo;
        private readonly IRepository<string, User> userrepo;
        private readonly IRepository<string, Manager> managerrepo;
        private readonly IRepository<string, Category> categoryrepo;
        private readonly IRepository<string, CategoryAddRequest> categaddrepo;
        private readonly IRepository<string, Product> prodrepo;
        private readonly IRepository<string, ProductUpdateLog> produpdrepo;
        private readonly IRepository<string, StockLogging> stockupdrepo;






        private readonly IEncryptService encryptService;

        public AdminService(IRepository<string, StockLogging> st, IRepository<string, ProductUpdateLog> prod, IRepository<string, Product> p, IRepository<string, CategoryAddRequest> cat, IRepository<string, Category> ca, ICurrentUserService cu, IManagerService ma, InventoryContext c, IEncryptService e, IRepository<string, Admin> a, IRepository<string, User> u, IRepository<string, Manager> m)
        {
            adminrepo = a;
            userrepo = u;
            managerrepo = m;
            encryptService = e;
            context = c;
            managerService = ma;
            currentUserService = cu;
            managerMapper = new();
            categoryrepo = ca;
            categaddrepo = cat;
            prodrepo = p;
            produpdrepo = prod;
            stockupdrepo = st;
        }
        public async Task<Admin> GetByMail(string mail)
        {
            var admin = await context.Admins.FirstOrDefaultAsync(a => a.Email == mail);
            return admin;
        }
        public async Task<Admin> AddAdmin(AdminManagerAddRequestDTO request)
        {
            var admin = await GetByMail(request.Email);
            if (admin == null)
            {
                var data = new EncryptModel();
                data.Data = request.Password;
                var encryptedData = await encryptService.EncryptData(data);
                Admin a = new();
                a.Id = await encryptService.GenerateId("ADM");
                a.Name = request.Name;
                a.Email = request.Email;
                a.Password = encryptedData.EncryptedData;

                User u = new();
                u.Id = a.Id;
                u.Email = a.Email;
                u.Password = a.Password;
                u.Role = "ADMIN";

                var user = await userrepo.Add(u);

                var ad = await adminrepo.Add(a);

                return a;

            }
            else
            {
                return null;
            }
        }
        public async Task<Manager> AddManager(AdminManagerAddRequestDTO request)
        {
            var manager = await managerService.GetByMail(request.Email);
            if (manager == null)
            {
                var creatorAdmin = await GetByMail(currentUserService.Email);

                var data = new EncryptModel();
                data.Data = request.Password;
                var encryptedData = await encryptService.EncryptData(data);
                Manager? m = managerMapper.MapManager(request);
                m.Id = await encryptService.GenerateId("MAN");
                m.Password = encryptedData.EncryptedData;
                m.CreatedBy = creatorAdmin.Id;

                User u = new();
                u.Id = m.Id;
                u.Email = m.Email;
                u.Password = m.Password;
                u.Role = "MANAGER";

                var user = await userrepo.Add(u);

                var ad = await managerrepo.Add(m);

                return m;

            }
            else
            {
                return null;
            }
        }
        public async Task<Category> AddCategory(string category)
        {
            category = category.ToUpper();
            var inrequests = await categaddrepo.GetByName(category);
            var exists = await categoryrepo.GetByName(category);
            if (exists == null)
            {
                var creatorAdmin = await GetByMail(currentUserService.Email);

                var categoryId = await encryptService.GenerateId("CAT");
                Category c = new();
                c.Id = categoryId;
                c.CategoryName = category.Trim();
                c.CreatedBy = creatorAdmin.Id;
                var cat = await categoryrepo.Add(c);

                if (inrequests != null)
                {
                    inrequests.Status = "ADDED";
                    await context.SaveChangesAsync();
                }
                return cat;
            }
            else
            {
                return null;
            }
        }
        public async Task<CategoryAddRequest> CancelCategoryAddrequest(string categaddreq)
        {
            categaddreq = categaddreq.ToUpper();
            var request = await categaddrepo.GetAll();
            var reqs_ = request.FirstOrDefault(r => r.CategoryName == categaddreq && r.Status == "REQUESTED");

            if (reqs_ == null)
            {
                return null;
            }
            else
            {
                reqs_.Status = "DENIED";
                await context.SaveChangesAsync();
                return reqs_;
            }

        }
        public async Task<Manager> DeleteManager(string ManagerId)
        {
            var manager = await managerrepo.GetById(ManagerId);
            manager.Status = "INACTIVE";

            await context.SaveChangesAsync();

            return manager;
        }
        public async Task<object> CheckManagerActivity(string id)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Manager Activity Report for ID: {id}");
            sb.AppendLine("--------------------------------------------------");

            var allprods = await prodrepo.GetAll();
            var prods_by_manager = allprods.Where(p => p.AddedBy == id).ToList();

            var allrequests = await categaddrepo.GetAll();
            var categ_req_by_manager = allrequests.Where(r => r.RequestedBy == id).ToList();

            var allstockupd = await stockupdrepo.GetAll();
            var stock_upd_by_manager = allstockupd.Where(u => u.UpdatedBy == id).ToList();

            var allprodupd = await produpdrepo.GetAll();
            var prod_upd_by_manager = allprodupd.Where(u => u.UpdatedBy == id).ToList();

            // if (prods_by_manager.Any())
            // {
            //     sb.AppendLine("\nProducts Created:");
            //     foreach (var prod in prods_by_manager)
            //     {
            //         sb.AppendLine($"   • Product ID: {prod.Id}, Name: {prod.Name}");
            //     }
            // }

            // if (categ_req_by_manager.Any())
            // {
            //     sb.AppendLine("\nCategory Addition Requests:");
            //     foreach (var req in categ_req_by_manager)
            //     {
            //         sb.AppendLine($"   • Request ID: {req.Id}, Category: {req.CategoryName}");
            //     }
            // }

            // if (stock_upd_by_manager.Any())
            // {
            //     sb.AppendLine("\nInventory Updates:");
            //     foreach (var s in stock_upd_by_manager)
            //     {
            //         sb.AppendLine($"   • Inventory ID: {s.InventoryId}, Stock changed from {s.OldStock} to {s.NewStock}");
            //     }
            // }

            // if (prod_upd_by_manager.Any())
            // {
            //     sb.AppendLine("\nProduct Updates:");
            //     foreach (var p in prod_upd_by_manager)
            //     {
            //         sb.AppendLine($"   • Product ID: {p.ProductId}, Field: {p.FieldUpdated}, New Value: {p.NewValue}");
            //     }
            // }

            // if (sb.Length == 0)
            // {
            //     sb.AppendLine("No activity found for this manager.");
            // }

            // return sb.ToString();

            var managersummary = new
            {
                ProductAdded = prods_by_manager.Select(p => new { p.Id, p.Name, p.Description, p.Price }),
                AllRequest = categ_req_by_manager.Select(c => new { c.CategoryName }),
                StockUpds = stock_upd_by_manager.Select(s => new { s.InventoryId, s.OldStock, s.NewStock }),
                ProdUpds = prod_upd_by_manager.Select(p => new { p.ProductId, p.FieldUpdated, p.NewValue }),
            };

            return managersummary;

        }

        public async Task<object> AdminActivity(string id)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Admin Activity Report for ID: {id}");
            sb.AppendLine("--------------------------------------------------");

            var allprods = await prodrepo.GetAll();
            var prods_by_admin = allprods.Where(p => p.AddedBy == id).ToList();

            var allrequests = await categoryrepo.GetAll();
            var categ_by_admin = allrequests.Where(r => r.CreatedBy == id).ToList();

            var allstockupd = await stockupdrepo.GetAll();
            var stock_upd_by_admin = allstockupd.Where(u => u.UpdatedBy == id).ToList();

            var allprodupd = await produpdrepo.GetAll();
            var prod_upd_by_admin = allprodupd.Where(u => u.UpdatedBy == id).ToList();

            var managers = await managerrepo.GetAll();
            var cur_user = await GetByMail(currentUserService.Email);

            var by_me = managers.Where(m => m.CreatedBy == cur_user.Id);


            // if (prods_by_admin.Any())
            // {
            //     sb.AppendLine("\nProducts Created:");
            //     foreach (var prod in prods_by_admin)
            //     {
            //         sb.AppendLine($"   • Product ID: {prod.Id}, Name: {prod.Name}");
            //     }
            // }

            // if (categ_by_admin.Any())
            // {
            //     sb.AppendLine("\nCategories Added");
            //     foreach (var req in categ_by_admin)
            //     {
            //         sb.AppendLine($"   • Request ID: {req.Id}, Category: {req.CategoryName}");
            //     }
            // }

            // if (stock_upd_by_admin.Any())
            // {
            //     sb.AppendLine("\nInventory Updates:");
            //     foreach (var s in stock_upd_by_admin)
            //     {
            //         sb.AppendLine($"   • Inventory ID: {s.InventoryId}, Stock changed from {s.OldStock} to {s.NewStock}");
            //     }
            // }

            // if (prod_upd_by_admin.Any())
            // {
            //     sb.AppendLine("\nProduct Updates:");
            //     foreach (var p in prod_upd_by_admin)
            //     {
            //         sb.AppendLine($"   • Product ID: {p.ProductId}, Field: {p.FieldUpdated}, New Value: {p.NewValue}");
            //     }
            // }
            // if (by_me.Any())
            // {
            //     sb.AppendLine("\nManagers created:");
            //     foreach (var p in by_me)
            //     {
            //         sb.AppendLine($"   • Manager ID: {p.Id}, Name: {p.Name}");
            //     }
            // }



            // if (sb.Length == 0)
            // {
            //     sb.AppendLine("No activity found for this admin.");
            // }

            // return sb.ToString();


            var adminsummary = new
            {
                ProdsAdded = prods_by_admin.Select(p => new { p.Id, p.Name, p.Description, p.Price }),
                Categadds = categ_by_admin.Select(c => new { c.Id, c.CategoryName }),
                StockUpds = stock_upd_by_admin.Select(s => new { s.InventoryId, s.OldStock, s.NewStock }),
                ProdUpds = prod_upd_by_admin.Select(p => new { p.ProductId, p.FieldUpdated, p.NewValue }),
                Managers = by_me.Select(m => new { m.Id, m.Name, m.Email })
            };

            return adminsummary;
        }

    }
    

}