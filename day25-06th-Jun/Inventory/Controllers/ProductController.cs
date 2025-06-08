using Inventory.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Inventory.Models;
using Inventory.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly IProductService prodService;

        private readonly IRepository<string, Category> categrepo;
        private readonly IRepository<string, Product> prodrepo;
        private readonly IRepository<string, Inventories> invrepo;
        private readonly IRepository<string, StockLogging> stockupdlogrepo;
        private readonly IRepository<string, ProductUpdateLog> produpdlogrepo;






        public ProductController(IRepository<string, ProductUpdateLog> prod, IRepository<string, StockLogging> st, IRepository<string, Inventories> i, IRepository<string, Product> pro, IProductService pr, IAdminService ad, IRepository<string, Category> ca)
        {
            adminService = ad;
            categrepo = ca;
            prodService = pr;
            prodrepo = pro;
            invrepo = i;
            stockupdlogrepo = st;
            produpdlogrepo = prod;
        }
        [HttpGet("Get-All-Categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var allcats = await categrepo.GetAll();
            return Ok(allcats);
        }
        [HttpGet("Get-Category-By-Name")]
        public async Task<IActionResult> GetCategoryName(string c)
        {
            var allcats = await categrepo.GetByName(c.ToUpper());
            return Ok(allcats);
        }

        [HttpPost("Add-Product")]
        public async Task<IActionResult> AddProduct(ProductAddRequest request)
        {
            var result = await prodService.AddProduct(request);
            if (result == null)
            {
                return BadRequest("Either category doesnt exists, or product already exists");
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpGet("Get-All-Products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var allprod = await prodrepo.GetAll();
            return Ok(allprod);
        }

        [HttpGet("Get-All-Active-Products")]
        public async Task<IActionResult> GetAllActiveProducts()
        {
            var allprod = await prodrepo.GetAll();
            var ative = allprod.Where(p => p.Status == "ACTIVE");
            return Ok(ative);
        }

        [HttpGet("Get-All-InActive-Products")]
        public async Task<IActionResult> GetAllInActiveProducts()
        {
            var allprod = await prodrepo.GetAll();
            var inactive = allprod.Where(p => p.Status == "INACTIVE");
            return Ok(inactive);
        }
        [HttpGet("Get-Product-By-Name")]
        public async Task<IActionResult> GetProductByName(string product)
        {
            var allprod = await prodrepo.GetByName(product);
            return Ok(allprod);
        }
        [HttpGet("Get-All-Inventory")]
        public async Task<IActionResult> GetAllInventories()
        {
            var allinv = await invrepo.GetAll();
            return Ok(allinv);
        }

        [HttpGet("Get-Stock-By-ProductName")]
        public async Task<IActionResult> GetStockProductName(string product)
        {
            var inv = await invrepo.GetByName(product);
            return Ok(inv);
        }


        [HttpPut("Stock-Update")]
        public async Task<IActionResult> StockUpdate(StockUpdateDTO request)
        {
            var result = await prodService.StockUpdate(request);
            if (result == null)
            {
                return BadRequest("Update Dismissed!");
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("Get-All-Stock-updates")]

        public async Task<IActionResult> GetAllStockUpdates()
        {
            var allupdates = await stockupdlogrepo.GetAll();
            return Ok(allupdates);
        }

        [HttpGet("Get-All-Stock-updates-For-ProductName")]
        public async Task<IActionResult> GetAllStockUpdatesForProduct(string product)
        {
            var allupdates = await stockupdlogrepo.GetAll();
            var inv_id = await invrepo.GetByName(product);
            var updatesofProduct = allupdates.Where(u => u.InventoryId == inv_id.Id);
            return Ok(updatesofProduct);
        }

        [HttpPut("Update-Product-price")]
        public async Task<IActionResult> UpdateProdByPrice(UpdateProductPriceDTO req)
        {
            var result = await prodService.UpdateProductByPrice(req);
            if (result != null)
                return Ok(result);
            else
                return BadRequest("Wrong Update request");
        }

        [HttpPut("Update-Product-description")]
        public async Task<IActionResult> UpdateProdByDesc(UpdateProductDescriptionDTO req)
        {
            var result = await prodService.UpdateProductByDescription(req);
            if (result != null)
                return Ok(result);
            else
                return BadRequest("Wrong Update request");
        }

        [HttpGet("Get-All-Product-Updates")]
        public async Task<IActionResult> GetAllProductUpdates()
        {
            var allupdates = await produpdlogrepo.GetAll();
            return Ok(allupdates);
        }

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

        [Authorize(Roles ="MANAGER, ADMIN")]
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
                return Ok(result);
            }
        }
    }
}