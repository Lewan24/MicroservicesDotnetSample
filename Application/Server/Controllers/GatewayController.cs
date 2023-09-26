using System.Text;
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
        _httpClient = httpClientFactory.CreateClient("AllowUntrustedRootHttpClient");
        _configuration = configuration;
    }

    #region Auth Service
    #region GET
    [HttpGet]
    [Route("auth/{*apipath}")]
    public async Task<IActionResult> HandleAuthGet(string apipath) => 
        await RedirectGetRequestToMicroservice($"{Microservices.Auth}/{apipath}", Microservices.Auth);

    [HttpGet]
    [Authorize]
    [Route("auth/authorized/{*apipath}")]
    public async Task<IActionResult> HandleGetAuthAuthorized(string apipath) => 
        await RedirectGetRequestToMicroservice($"{Microservices.Auth}/authorized/{apipath}", Microservices.Auth);
    #endregion
    #endregion
    
    #region OrderSystem Service
    #region GET
    [HttpGet]
    [Route("ordersystem/{*apipath}")]
    public async Task<IActionResult> HandleGetOrderSystem(string apipath) => 
        await RedirectGetRequestToMicroservice($"{Microservices.OrdersSystem}/{apipath}", Microservices.OrdersSystem);
    #endregion
    #region POST
    [HttpPost]
    [Route("orderssystem/authorized/{*apipath}")]
    public async Task<IActionResult> HandlePostOrderSystem(string apipath, [FromBody] object? data) => 
        await RedirectPostRequestToMicroservice($"{Microservices.OrdersSystem}/authorized/{apipath}", Microservices.OrdersSystem, data);
    #endregion
    #endregion
    
    private async Task<IActionResult> RedirectGetRequestToMicroservice(string? apiPath, Microservices microservice)
    {
        var microserviceUrl = GetMicroserviceUrlBasedOnName(microservice);
        
        if (microserviceUrl is null) return NotFound();
        
        var queryString = HttpContext.Request.QueryString.ToString();

        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{microserviceUrl}/api/{apiPath}{queryString}")
        };

        //httpRequestMessage.Headers.Add("X-Client-Certificate", Convert.ToBase64String(_certificate.RawData));
        
        using var response = await _httpClient.SendAsync(httpRequestMessage);

        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
            
        return Ok(content);
    }
    
    private async Task<IActionResult> RedirectPostRequestToMicroservice(string? apiPath, Microservices microservice, object? data)
    {
        var microserviceUrl = GetMicroserviceUrlBasedOnName(microservice);
        if (microserviceUrl is null) 
            return NotFound();

        if (data is null)
            return BadRequest("Empty data");
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{microserviceUrl}/api/{apiPath}"),
            Content = new StringContent(data.ToString()!, Encoding.UTF8, "application/json")
        };

        using var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) 
            return StatusCode((int)response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        
        return Ok(content);
    }

    private string? GetMicroserviceUrlBasedOnName(Microservices request) => request switch
    {
        Microservices.Auth => _configuration["MicroservicesConnections:Auth"],
        Microservices.OrdersSystem => _configuration["MicroservicesConnections:OrdersSystem"],
        _ => null
    };

    private enum Microservices
    {
        Auth,
        OrdersSystem
    }
}
