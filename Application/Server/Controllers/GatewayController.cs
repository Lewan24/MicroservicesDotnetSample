using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Server.Controllers;

[Route("api")]
[ApiController]
public class GatewayController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GatewayController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
    }

    [HttpGet]
    [Route("auth/{*apipath}")]
    public async Task<IActionResult> HandleAuthGet(string apipath)
    {
        var microserviceUrl = GetMicroserviceUrlBasedOnName(Microservices.Auth);

        return await RedirectRequestToMicroservice(microserviceUrl, $"{Microservices.Auth}/{apipath}");
    }

    [HttpGet]
    [Authorize]
    [Route("auth/authorized/{*apipath}")]
    public async Task<IActionResult> HandleAuthAuthorizedGet(string apipath)
    {
        var microserviceUrl = GetMicroserviceUrlBasedOnName(Microservices.Auth);

        return await RedirectRequestToMicroservice(microserviceUrl, $"{Microservices.Auth}/authorized/{apipath}");
    }

    [HttpGet]
    [Route("ordersystem/{*apipath}")]
    public async Task<IActionResult> HandleOrderSystemGet(string? token, string apipath)
    {
        var microserviceUrl = GetMicroserviceUrlBasedOnName(Microservices.OrdersSystem);

        return await RedirectRequestToMicroservice(microserviceUrl, $"{Microservices.OrdersSystem}/{apipath}");
    }

    private async Task<IActionResult> RedirectRequestToMicroservice(string? microserviceUrl, string? apiPath)
    {
        if (microserviceUrl is not null)
        {
            var queryString = HttpContext.Request.QueryString.ToString();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{microserviceUrl}/api/{apiPath}{queryString}")
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Ok(content);
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
        }
        else
        {
            return NotFound();
        }
    }

    private string? GetMicroserviceUrlBasedOnName(Microservices request) => request switch
    {
        Microservices.Auth => _configuration["MicroservicesConnections:Auth"],
        Microservices.OrdersSystem => _configuration["MicroservicesConnections:OrdersSystem"],
        _ => null
    };

    public enum Microservices
    {
        Auth,
        OrdersSystem
    }
}
