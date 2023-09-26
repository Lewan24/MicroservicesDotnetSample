using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Filters;

public class AddHeaderFilter : IResultFilter
{
    private string? _hosts = string.Empty;
    
    public AddHeaderFilter(){}
    
    public AddHeaderFilter(string? hosts)
    {
        _hosts = hosts;
    }
    
    public void OnResultExecuted(ResultExecutedContext context)
    {
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.HttpContext.Response is not { } response) return;
        
        response.Headers.Add("Access-Control-Allow-Origin", string.IsNullOrWhiteSpace(_hosts) ? "*" : _hosts);
        response.Headers.Add("Access-Control-Allow-Headers","*");
        response.Headers.Add("Access-Control-Allow-Methods","*");
    }
}