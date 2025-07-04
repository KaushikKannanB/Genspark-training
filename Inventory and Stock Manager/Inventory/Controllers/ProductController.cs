using Inventory.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Inventory.Models;
using Inventory.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Inventory.Hubs;
using System.Text;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly IManagerService managerService;
        private readonly ICurrentUserService currentUserService;

        private readonly IProductService prodService;
        private readonly IHubContext<NotificationHub> hubContext;

        private readonly IRepository<string, Category> categrepo;
        private readonly IRepository<string, Product> prodrepo;
        private readonly IRepository<string, Inventories> invrepo;
        private readonly IRepository<string, StockLogging> stockupdlogrepo;
        private readonly IRepository<string, ProductUpdateLog> produpdlogrepo;






        public ProductController(IManagerService man, ICurrentUserService cu, IHubContext<NotificationHub> hub, IRepository<string, ProductUpdateLog> prod, IRepository<string, StockLogging> st, IRepository<string, Inventories> i, IRepository<string, Product> pro, IProductService pr, IAdminService ad, IRepository<string, Category> ca)
        {
            adminService = ad;
            categrepo = ca;
            prodService = pr;
            prodrepo = pro;
            invrepo = i;
            stockupdlogrepo = st;
            produpdlogrepo = prod;
            hubContext = hub;
            currentUserService = cu;
            managerService = man;
        }

        [Authorize]
        [HttpPost("Add-Product")]
        public async Task<IActionResult> AddProduct(ProductAddRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await prodService.AddProduct(request);
            if (result == null)
            {
                return BadRequest("Either category doesnt exists, or product already exists");
            }
            else
            {
                var cur_user_admin = await adminService.GetByMail(currentUserService.Email);
                var cur_user_manager = await managerService.GetByMail(currentUserService.Email);
                if (cur_user_admin != null)
                {
                    await hubContext.Clients.All.SendAsync(
                        "ReceiveNotification",
                        $"Product: {request.Name.ToUpper()} added by Admin {cur_user_admin.Id}: {cur_user_admin.Name.ToUpper()} --> notified at {DateTime.UtcNow}"
                    );
                }
                else if (cur_user_manager != null)
                {
                    await hubContext.Clients.All.SendAsync(
                        "ReceiveNotification",
                        $"Product: {request.Name.ToUpper()} added by Manager {cur_user_manager.Id}: {cur_user_manager.Name.ToUpper()} --> notified at {DateTime.UtcNow}"
                    );
                }
                return Ok(result);
            }
        }
        [Authorize]
        [HttpGet("Get-All-Categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var allcats = await categrepo.GetAll();
            return Ok(allcats);
        }
        [Authorize]
        [HttpGet("Get-product-by-inventory-id")]
        public async Task<IActionResult> GetProdByInvId(string id)
        {
            var allprods = await prodrepo.GetAll();
            var myprod = allprods.Where(p => p.InventoryId == id);
            return Ok(myprod);

        }
        [Authorize]
        [HttpGet("Get-Category-By-Name")]
        public async Task<IActionResult> GetCategoryName(string c)
        {
            var allcats = await categrepo.GetByName(c.ToUpper());
            return Ok(allcats);
        }
        [Authorize]
        [HttpGet("Get-category-by-Id")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var allcats = await categrepo.GetById(id);
            return Ok(allcats);
        }
        [Authorize]
        [HttpGet("Get-All-Products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var allprod = await prodrepo.GetAll();
            return Ok(allprod);
        }
        [Authorize]
        [HttpGet("Get-Productby-Id")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var prod = await prodrepo.GetById(id);
            return Ok(prod);
        }

        [Authorize]
        [HttpGet("Get-Products-Orderedby-Price")]
        public async Task<IActionResult> GetProductsOrdered(int pagenumber)
        {
            if (pagenumber <= 0)
            {
                return BadRequest("Meaningful page number required!");
            }
            var prods = await prodService.GetProductsPaginated(pagenumber);
            if (prods == null || prods.Count() == 0)
            {
                var allprod = await prodrepo.GetAll();
                return BadRequest($"There are only {allprod.Count()} products.");
            }
            else
            {
                return Ok(prods);
            }
        }

        [Authorize]
        [HttpGet("Get-All-Active-Products")]
        public async Task<IActionResult> GetAllActiveProducts()
        {
            var allprod = await prodrepo.GetAll();
            var ative = allprod.Where(p => p.Status == "ACTIVE");
            return Ok(ative);
        }

        [Authorize]
        [HttpGet("Get-All-InActive-Products")]
        public async Task<IActionResult> GetAllInActiveProducts()
        {
            var allprod = await prodrepo.GetAll();
            var inactive = allprod.Where(p => p.Status == "INACTIVE");
            return Ok(inactive);
        }
        [Authorize]
        [HttpGet("Get-Product-By-Name")]
        public async Task<IActionResult> GetProductByName(string product)
        {
            var allprod = await prodrepo.GetByName(product);
            return Ok(allprod);
        }
        [Authorize]
        [HttpGet("Get-All-Inventory")]
        public async Task<IActionResult> GetAllInventories()
        {
            var allinv = await invrepo.GetAll();
            return Ok(allinv);
        }
        [Authorize]
        [HttpGet("Get-Stock-By-ProductName")]
        public async Task<IActionResult> GetStockProductName(string product)
        {
            var inv = await invrepo.GetByName(product);
            return Ok(inv);
        }



        [Authorize]
        [HttpGet("Get-All-Stock-updates")]

        public async Task<IActionResult> GetAllStockUpdates()
        {
            var allupdates = await stockupdlogrepo.GetAll();
            return Ok(allupdates);
        }
        [Authorize]
        [HttpGet("Get-All-Stock-updates-For-ProductName")]
        public async Task<IActionResult> GetAllStockUpdatesForProduct(string product)
        {
            var allupdates = await stockupdlogrepo.GetAll();
            var inv_id = await invrepo.GetByName(product);
            var updatesofProduct = allupdates.Where(u => u.InventoryId == inv_id.Id);
            return Ok(updatesofProduct);
        }




        [Authorize]
        [HttpGet("Get-All-Product-Updates")]
        public async Task<IActionResult> GetAllProductUpdates()
        {
            var allupdates = await produpdlogrepo.GetAll();
            return Ok(allupdates);
        }
        [Authorize]
        [HttpGet("Get-All-Product-Updates-By-ProductName")]
        public async Task<IActionResult> GetAllProductUpdatesByName(string product)
        {
            var allupdates = await produpdlogrepo.GetAll();
            var prod = await prodrepo.GetByName(product);
            var allupdatesProd = allupdates.Where(p => p.ProductId == prod.Id);

            if (allupdatesProd != null)
                return Ok(allupdatesProd);
            else
                return BadRequest("No updates so far");
        }
        [Authorize]
        [HttpGet("Get-Filtered-Products")]
        public async Task<IActionResult> GetAllProductsFiltered(string? categoryname, float? minprice, float? maxprice, string? status)
        {

            var prods = await prodrepo.GetAll();
            if (!string.IsNullOrEmpty(categoryname))
            {
                var catid = await categrepo.GetByName(categoryname);
                prods = prods.Where(p => p.CategoryId == catid.Id);
            }
            if (minprice.HasValue)
            {
                prods = prods.Where(p => p.Price > minprice);
            }
            if (maxprice.HasValue)
            {
                prods = prods.Where(p => p.Price <= maxprice);
            }
            if (!string.IsNullOrEmpty(status))
            {
                prods = prods.Where(p => p.Status == status);
            }

            return Ok(prods);
        }
        [Authorize]
        [HttpPut("Stock-Update")]
        public async Task<IActionResult> StockUpdate(StockUpdateDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await prodService.StockUpdate(request);
            if (request == null)
            {
                return BadRequest("Request body is NULL");
            }
            if (result == null)
            {
                return BadRequest("Update Dismissed!");
            }
            else
            {
                if (result.Stock <= result.MinThreshold)
                {
                    await hubContext.Clients.All.SendAsync("ReceiveNotification", $"Stock Needs to be refilled for Product {request.ProductName}  ASAP --> notified at {DateTime.UtcNow}");
                }
                return Ok(result);
            }
        }
        [Authorize]
        [HttpPut("Update-Product-description")]
        public async Task<IActionResult> UpdateProdByDesc(UpdateProductDescriptionDTO req)
        {
            var result = await prodService.UpdateProductByDescription(req);
            if (result != null)
                return Ok(result);
            else
                return BadRequest("Wrong Update request");
        }
        [Authorize]
        [HttpPut("Update-Product-price")]
        public async Task<IActionResult> UpdateProdByPrice(UpdateProductPriceDTO req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await prodService.UpdateProductByPrice(req);
            if (result != null)
                return Ok(result);
            else
                return BadRequest("Wrong Update request");
        }
        [Authorize(Roles = "MANAGER, ADMIN")]
        [HttpDelete("Delete-Product")]
        public async Task<IActionResult> Deleteproduct(string product)
        {
            var result = await prodService.DeleteProduct(product);
            if (result == null)
            {

                return BadRequest("Could not delete");
            }
            else
            {
                var cur_user_admin = await adminService.GetByMail(currentUserService.Email);
                var cur_user_manager = await managerService.GetByMail(currentUserService.Email);
                if (cur_user_admin != null)
                {
                    await hubContext.Clients.All.SendAsync("ReceiveNotification", $"Product: {product.ToUpper()} deleted by ${cur_user_admin.Id}: ${cur_user_admin.Name.ToUpper()} --> notified at {DateTime.UtcNow}");

                }
                else if (cur_user_manager != null)
                {
                    await hubContext.Clients.All.SendAsync("ReceiveNotification", $"Product: {product.ToUpper()} deleted by ${cur_user_manager.Id}: ${cur_user_manager.Name.ToUpper()} --> notified at {DateTime.UtcNow}");

                }
                // var cur_user = await adminService.GetByMail(currentUserService.Email);
                return Ok(result);
            }
        }
        [Authorize]
        [HttpGet("Get-product-summary")]
        public async Task<IActionResult> GetProductSummary(string prod)
        {
            prod = prod.ToUpper();
            var prodbasicdeets = await prodrepo.GetByName(prod);
            var invid = prodbasicdeets.InventoryId;
            var allstockupds = await stockupdlogrepo.GetAll();
            var myupdsstock = allstockupds.Where(s => s.InventoryId == invid);
            var allprodupds = await produpdlogrepo.GetAll();
            var myupdsprod = allprodupds.Where(p => p.ProductId == prodbasicdeets.Id);
            var currentstock = await invrepo.GetById(invid);

            var prodsummary = new
            {
                ProdDetails = prodbasicdeets,
                CurrentInv = currentstock,
                StockLogs = myupdsstock,
                ProdLogs = myupdsprod
            };
            return Ok(prodsummary);
            
        }
    }
}