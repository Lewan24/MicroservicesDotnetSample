using Microsoft.AspNetCore.Mvc;

namespace OrdersSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersSystemController : ControllerBase
{
    [HttpGet]
    [Route("Check")]
    public IActionResult CheckController() => 
        Ok("Success from Order System");
}
