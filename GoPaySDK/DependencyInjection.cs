using GoPaySDK.Interfaces;
using GoPaySDK.Services;
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
    public static IServiceCollection AddGoPayService(this IServiceCollection services, string apiKey, string secretKey)
    {
        if(string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentNullException(nameof(apiKey));
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new ArgumentNullException(nameof(secretKey));

        Variables.ApiKey = apiKey;
        Variables.SecretKey = secretKey;
        services.AddHttpClient(Variables.GoPay, httpClient =>
        {
            httpClient.BaseAddress = new Uri(Variables.BaseUrl);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("gopay-api-key", Variables.ApiKey);
        });
        services.AddScoped<IGoPayService, GoPayService>();
        return services;
    }
}
