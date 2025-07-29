using MainMigration.Interfaces;
using MainMigration.Models;
using MainMigration.Models.DTOs;
using MainMigration.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainMigration.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IRepository<int, Product> prodrepo;

        public ProductController(IProductService pr, IRepository<int, Product> p)
        {
            productService = pr;
            prodrepo = p;
        }
        [HttpPost("add-product-to-sell")]
        public async Task<IActionResult> AddProduct(AddProductDTO req)
        {
            var res = await productService.AddProduct(req);
            if (res == null)
            {
                return BadRequest("Invalid request");
            }
            return Ok("Product added");
        }

        [HttpGet("get-by-product-name-unsold_only")]
        public async Task<IActionResult> GetProduct(string name)
        {
            var res = await productService.GetByproductname(name);
            if (res == null)
            {
                return BadRequest("No such product exist!");
            }
            return Ok(res);
        }

        [HttpGet("get-all-unsold-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var all = await prodrepo.GetAll();
            all = all.Where(p => p.isSold == "NO");

            return Ok(all);
        }

        [HttpGet("get-filtered-products_unsold_only")]
        public async Task<IActionResult> GetFilteredProducts(string? cat, string? col, string? mod, string? prodname)
        {
            var res = await productService.GetFilteredProducts(cat?.ToLower(), col?.ToLower(), mod?.ToLower(), prodname?.ToLower());

            if (res == null)
            {
                return BadRequest("No such products");
            }
            return Ok(res);
            
        }

    }
}