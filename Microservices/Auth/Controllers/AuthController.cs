using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet]
    [Route("Check")]
    public IActionResult CheckController() => 
        Ok("Success from Auth controller");

    [HttpGet]
    [Route("Check/Test")]
    public IActionResult CheckTestController() =>
        Ok("Success from Auth controller - Check test");

    [HttpGet]
    [Route("Checkname")]
    public IActionResult CheckNameController([FromQuery] string? name) =>
        Ok($"Success from Auth controller - Hello {name}");

    [HttpGet]
    [Route("authorized/Checkname")]
    public IActionResult CheckNameAuthorizedController([FromQuery] string? name) =>
        Ok($"Success from Auth controller - Hello {name}");
}
