using MainMigration.Interfaces;
using MainMigration.Models;
using Microsoft.AspNetCore.Mvc;

namespace MainMigration.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService or)
        {
            orderService = or;
        }

        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder(IEnumerable<int> productids)
        {
            var ordered = await orderService.PlaceOrder(productids);

            return Ok(ordered);
        }

        [HttpGet("get-my-orders")]

        public async Task<IActionResult> GetMyorders()
        {
            var ordered = await orderService.GetMyOrders();

            return Ok(ordered);
        }

        [HttpGet("get-my-order-details")]
        public async Task<IActionResult> GetMyOrderDetails(int orderid)
        {
            var deets = await orderService.GetMyOrderDetails(orderid);
            return Ok(deets);
        }
    }
}