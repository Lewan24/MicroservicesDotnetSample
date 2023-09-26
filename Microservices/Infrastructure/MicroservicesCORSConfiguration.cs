using Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class MicroservicesCorsConfiguration
{
    /// <summary>
    /// Adds CORS configuration that allows for every method and every header, but only for specific origins. IF Origins is null or empty, then allows for every origin.
    /// </summary>
    /// <param name="services">Services collection where the cors will be added</param>
    /// <param name="originsToAllow">Hosts as string[], these hosts will be allowed to webapi</param>
    public static void AddCustomCorsConfiguration(this IServiceCollection services, string[]? originsToAllow)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (originsToAllow is null)
                    policy.WithOrigins("*");
                else
                    policy.WithOrigins(originsToAllow);

                policy.AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    /// <summary>
    /// Add filter that allows selected origins, methods and headers in webapi. This is additional cors allowing method. Default CORS is not working everytime.
    /// </summary>
    /// <param name="services">Services collection where the filter will be added</param>
    /// <param name="hostsToAllow">Hosts as string, where every next host should be wrote after ','</param>
    public static void AddCustomCorsHeaderFilter(this IServiceCollection services, string? hostsToAllow)
    {
        services.Configure<MvcOptions>(opt =>
        {
            opt.Filters.Add(new AddHeaderFilter(hostsToAllow));
        });
    }
}