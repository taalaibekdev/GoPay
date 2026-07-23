using GoPaySDK.Interfaces;
using GoPaySDK.Models;
using GoPaySDK.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoPaySDK;

public static class DependencyInjection
{
    /// <summary>
    /// AddGoPayServices
    /// After you can use IGoPayService
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration</param>
    /// <returns>IServiceCollection with GoPayService registered</returns>
    public static IServiceCollection AddGoPayService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GoPayOptions>(configuration.GetSection(Variables.GoPay));
        services.AddHttpClient<IGoPayService, GoPayService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        });
        return services;
    }
}
