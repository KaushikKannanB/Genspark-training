using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class SampleController : ControllerBase
{
    [HttpGet("get")]
    public ActionResult GetString()
    {
        return Ok("hello application");
    }
}

