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
    /// <param name="apiKey">ApiKey from GoPay KG</param>
    /// <param name="secretKey">SecretKey from GoPay KG</param>
    /// <returns></returns>
    public static IServiceCollection AddGoPayService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GoPayOptions>(configuration.GetSection(Variables.GoPay));
        services.AddHttpClient();
        services.AddScoped<IGoPayService, GoPayService>();
        return services;
    }
}
