using GoPaySDK;
using GoPaySDK.Interfaces;
using GoPaySDK.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();
var _goPayService = scope.ServiceProvider.GetRequiredService<IGoPayService>();

var paymentResult = await _goPayService.CreatePaymentAsync(new CreatePayment
{
    order_id = Guid.CreateVersion7().ToGoPayOrderId(),
    amount = 5,
    description = "Test Payment",
    testing_mode = true,
});

if (paymentResult.status == ResponseMessages.StatusOK)
{
    var payment = paymentResult.data;
    Console.WriteLine($"Payment created with ID: {payment.payment_id}");
}
else
{
    Console.WriteLine($"Error creating payment: {paymentResult.error_message}");
}

Console.WriteLine("Process completed");
Console.ReadKey();
static IHostBuilder CreateHostBuilder(string[] args) =>
Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        context.HostingEnvironment.EnvironmentName = Environments.Development;

        if (context.HostingEnvironment.IsDevelopment())
        {
            config.AddUserSecrets<Program>();
        }
    })
    .ConfigureServices((context, services) =>
    {
        var apiKey = context.Configuration["GoPay:ApiKey"]!;
        var secretKey = context.Configuration["GoPay:SecretKey"]!;
        services.AddGoPayService(apiKey, secretKey);
    });