using Application.Shared.Models;
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

    [HttpPost]
    [Route("authorized/createorder")]
    public IActionResult CreateOrder([FromBody] OrderModelDto? data)
    {
        try
        {
            if (data is null)
                return BadRequest("Bad data. Data is null");

            var random = new Random();
            return Ok(new OrderModel(Guid.NewGuid(), data.CustomerName, data.NumberOfProducts <= 0 ? random.Next(0, 99) : data.NumberOfProducts));
        }
        catch (Exception e)
        {
            return BadRequest($"Error on creating order. Error: {e.Message}");
        }
    }
}