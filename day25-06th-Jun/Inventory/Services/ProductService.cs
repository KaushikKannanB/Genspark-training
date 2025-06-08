using Inventory.Contexts;
using Inventory.Interfaces;
using Inventory.Misc;
using Inventory.Models;
using Inventory.Models.DTOs;

namespace Inventory.Services
{
    public class ProductService : IProductService
    {
        ProductMapper productMapper;
        private readonly InventoryContext context;
        private readonly IEncryptService encryptService;
        private readonly ICurrentUserService currentUserService;
        private readonly IUserService userService;


        private readonly IRepository<string, Product> prodrepo;
        private readonly IRepository<string, Manager> managerrepo;

        private readonly IRepository<string, Inventories> invrepo;
        private readonly IRepository<string, Category> categrepo;
        private readonly IRepository<string, StockLogging> stockupdlogrepo;
        private readonly IRepository<string, ProductUpdateLog> produpdlogrepo;



        public ProductService(IRepository<string, ProductUpdateLog> pro, IRepository<string, StockLogging> st, InventoryContext c, IUserService us, ICurrentUserService cu, IRepository<string, Inventories> i, IRepository<string, Product> pr, IRepository<string, Category> ca, IEncryptService en)
        {
            prodrepo = pr;
            categrepo = ca;
            encryptService = en;
            invrepo = i;
            productMapper = new();
            currentUserService = cu;
            userService = us;
            context = c;
            stockupdlogrepo = st;
            produpdlogrepo = pro;
        }
        public async Task<Product> AddProduct(ProductAddRequest request)
        {
            request.categoryName = request.categoryName.ToUpper();
            var exist_cat = await categrepo.GetByName(request.categoryName);
            if (exist_cat != null)
            {
                request.Name = request.Name.ToUpper();
                var exists_prod = await prodrepo.GetByName(request.Name);
                if (exists_prod == null)
                {
                    var cur_user = await userService.GetByMail(currentUserService.Email);
                    if (cur_user.Role == "MANAGER")
                    {
                        var manager = await managerrepo.GetById(cur_user.Id);
                        if (manager.Status == "INACTIVE")
                        {
                            return null;
                        }
                    }
                    var inv_id = await encryptService.GenerateId("INV");
                    var prod_id = await encryptService.GenerateId("PROD");

                    Inventories inv = new();
                    inv.Id = inv_id;
                    inv.Stock = request.stock;
                    inv.MinThreshold = request.MinThreshold;

                    var inv_added = await invrepo.Add(inv);

                    Product? p = productMapper.MapProduct(request);
                    p.CategoryId = exist_cat.Id;
                    p.InventoryId = inv_added.Id;
                    p.AddedBy = cur_user.Id;
                    p.Id = prod_id;

                    var prod_added = await prodrepo.Add(p);
                    return prod_added;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public async Task<Inventories> StockUpdate(StockUpdateDTO request)
        {
            try
            {
                request.ProductName = request.ProductName.ToUpper().Trim();
                request.AddOrReduce = request.AddOrReduce.ToUpper().Trim();
                var prod_exists = await invrepo.GetByName(request.ProductName);
                var cur_user = await userService.GetByMail(currentUserService.Email);
                if (cur_user.Role == "MANAGER")
                {
                    var manager = await managerrepo.GetById(cur_user.Id);
                    if (manager.Status == "INACTIVE")
                    {
                        return null;
                    }
                }
                if (prod_exists != null)
                {
                    var productdeets = await prodrepo.GetByName(request.ProductName);
                    if (productdeets.Status == "INACTIVE")
                    {
                        return null;
                    }
                    var oldstock = prod_exists.Stock;
                    if (request.AddOrReduce == "ADD")
                    {
                        prod_exists.Stock += request.AddOrReduceBy;
                    }
                    else if (request.AddOrReduce == "REDUCE" && request.AddOrReduceBy < prod_exists.Stock)
                    {
                        prod_exists.Stock -= request.AddOrReduceBy;
                    }
                    else
                    {
                        throw new Exception("Invalid Stock changes...dismissed!");
                    }

                    await context.SaveChangesAsync();
                    var stock_upd_id = await encryptService.GenerateId("STOCK_UPD");
                    StockLogging st = new();
                    st.Id = stock_upd_id;
                    st.InventoryId = prod_exists.Id;
                    st.OldStock = oldstock;
                    st.NewStock = prod_exists.Stock;
                    st.UpdatedBy = cur_user.Id;

                    var stock_upd = await stockupdlogrepo.Add(st);

                    return prod_exists;
                }
                else
                {
                    throw new Exception("Product doesnt Exists!");
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<Product> UpdateProductByPrice(UpdateProductPriceDTO request)
        {
            request.ProductName = request.ProductName.ToUpper().Trim();
            var prod_exists = await prodrepo.GetByName(request.ProductName);

            if (prod_exists != null)
            {
                if (prod_exists.Status == "INACTIVE")
                {
                    return null;
                }
                var oldprice = prod_exists.Price;
                prod_exists.Price = request.newprice;
                await context.SaveChangesAsync();

                var pr_log_id = await encryptService.GenerateId("PROD_UPD");
                var cur_user = await userService.GetByMail(currentUserService.Email);
                if (cur_user.Role == "MANAGER")
                {
                    var manager = await managerrepo.GetById(cur_user.Id);
                    if (manager.Status == "INACTIVE")
                    {
                        return null;
                    }
                }
                ProductUpdateLog plog = new();
                plog.Id = pr_log_id;
                plog.FieldUpdated = "PRICE";
                plog.ProductId = prod_exists.Id;
                plog.OldValue = oldprice.ToString();
                plog.NewValue = prod_exists.Price.ToString();
                plog.UpdatedBy = cur_user.Id;

                var upd_log = await produpdlogrepo.Add(plog);
                return prod_exists;
            }
            else
            {
                return null;
            }
        }

        public async Task<Product> UpdateProductByDescription(UpdateProductDescriptionDTO request)
        {
            request.ProductName = request.ProductName.ToUpper().Trim();
            var prod_exists = await prodrepo.GetByName(request.ProductName);

            if (prod_exists != null)
            {
                if (prod_exists.Status == "INACTIVE")
                {
                    return null;
                }
                var olddesc = prod_exists.Description;
                prod_exists.Description = request.NewDescription;
                await context.SaveChangesAsync();

                var pr_log_id = await encryptService.GenerateId("PROD_UPD");
                var cur_user = await userService.GetByMail(currentUserService.Email);
                if (cur_user.Role == "MANAGER")
                {
                    var manager = await managerrepo.GetById(cur_user.Id);
                    if (manager.Status == "INACTIVE")
                    {
                        return null;
                    }
                }
                ProductUpdateLog plog = new();
                plog.Id = pr_log_id;
                plog.FieldUpdated = "DESCRIPTION";
                plog.ProductId = prod_exists.Id;

                plog.OldValue = olddesc;
                plog.NewValue = prod_exists.Description;
                plog.UpdatedBy = cur_user.Id;

                var upd_log = await produpdlogrepo.Add(plog);
                return prod_exists;
            }
            else
            {
                return null;
            }
        }

        public async Task<Product> DeleteProduct(string product)
        {
            product = product.ToUpper();
            var prod_exists = await prodrepo.GetByName(product);
            if (prod_exists != null)
            {
                var cur_user = await userService.GetByMail(currentUserService.Email);
                if (cur_user.Role == "MANAGER")
                {
                    var manager = await managerrepo.GetById(cur_user.Id);
                    if (manager.Status == "INACTIVE")
                    {
                        return null;
                    }
                }

                prod_exists.Status = "INACTIVE";
                await context.SaveChangesAsync();
                return prod_exists;
            }
            else
            {
                return null;
            }
        }
    }
}