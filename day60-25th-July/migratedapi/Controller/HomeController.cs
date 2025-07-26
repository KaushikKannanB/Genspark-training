using Microsoft.AspNetCore.Mvc;

namespace MigratedAPI.Controllers
{
    [ApiController]
    [Route("api/sample")]
    public class HomeController : ControllerBase
    {
        [HttpGet("about")]
        public ActionResult About()
        {
            return Ok("This is yout About page");
        }
        [HttpGet("contact")]

        public ActionResult Contact()
        {
            return Ok("This is contact page");
        }
    }
}