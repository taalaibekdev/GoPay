using GoPaySDK;
using GoPaySDK.Interfaces;
using GoPaySDK.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();
var _goPayService = scope.ServiceProvider.GetRequiredService<IGoPayService>();

//Создание платежа
var paymentResult = await _goPayService.CreatePaymentAsync(new CreatePayment
{
    order_id = Guid.CreateVersion7().AdoptToGoPay(),
    amount = 1,
    description = "Test Payment",
    testing_mode = true,
});

//Проверка оплаты по ID платежа и ID заказа
//var paymentResult = await _goPayService.QueryPaymentAsync(new QueryPayment()
//{
//    order_id = Guid.Parse("019c70ec-75bc-7ec5-ab5f-8b21f9e9b2e7").AdoptToGoPay(),
//    payment_id = Guid.Parse("44176e59-fb4a-4d07-a696-774ade29a2c2").AdoptToGoPay()
//});

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
        services.AddGoPayService(context.Configuration);
    });