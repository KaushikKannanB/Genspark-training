using System.Globalization;
using MainMigration.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainMigration.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        public CartController(ICartService ca)
        {
            cartService = ca;
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> Addtocart(int productid)
        {
            var added = await cartService.AddtoCart(productid);
            return Ok(added);
        }

        [HttpGet("my-cart")]
        public async Task<IActionResult> ViewMyCart()
        {
            var mycart = await cartService.GetMyCart();
            if (mycart == null)
            {
                return BadRequest("Nothing in ur cart");
            }
            return Ok(mycart);
        }

        [HttpGet("exist-in-cart")]
        public async Task<IActionResult> ExistinCart(int productid)
        {
            var cart = await cartService.ExistinCart(productid);
            if (cart == null)
            {
                return BadRequest("Does not exist in cart");
            }
            else
            {
                return Ok(cart);
            }
        }

        [HttpGet("dispatch-my-cart")]
        public async Task<IActionResult> BuyMyCart()
        {
            var ordered = await cartService.BuyAllCart();
            return Ok(ordered);
        }

        [HttpDelete("remove-item-from-cart")]
        public async Task<IActionResult> Removefromcart(int productid)
        {
            var removed = await cartService.RemoveItemFromCart(productid);

            if (removed != null)
            {
                return Ok("Item removed from Cart");
            }
            return BadRequest("No such proeduct exist in your cart");
        }

        [HttpDelete("empty-cart")]
        public async Task<IActionResult> EmptyCart()
        {
            var cart = await cartService.EmptyCart();
            return Ok("Cart is empty now!");
        }

        [HttpPost("buy-this-product-from-cart")]
        public async Task<IActionResult> BuythisCart(int productid)
        {
            var bought = await cartService.BuySpecificItemFromCart(productid);
            return Ok(bought);
        }
    }
}