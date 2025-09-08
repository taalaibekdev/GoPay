using GoPaySDK.Interfaces;
using GoPaySDK.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoPaySDK;

public static class DependencyInjection
{
    public static IServiceCollection AddGoPayServices(this IServiceCollection services, IConfiguration configuration)
    {
        Variables.GoPayApiKey = configuration["GoPay:ApiKey"]!;
        Variables.GoPaySecretKey = configuration["GoPay:SecretKey"]!;

        services.AddScoped<IPaymentService, PaymentService>();
        return services;
    }
}
