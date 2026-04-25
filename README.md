# GoPay SDK for .NET

GoPay SDK — библиотека для интеграции GoPay KG в .NET-приложения. SDK помогает создавать платежи и проверять статус платежа через `IGoPayService`.

## Возможности

- Создание платежа в GoPay
- Проверка платежа по `payment_id` или `order_id`
- Поддержка Dependency Injection
- Настройка через `IConfiguration`
- Подходит для ASP.NET Core и Console-приложений

## Требования

- .NET 10 или выше
- `ApiKey` и `SecretKey` от GoPay KG
- `BaseUrl` API GoPay

## Установка

Сейчас библиотека не опубликована в NuGet. Используйте SDK как проект внутри своего решения.

Добавьте ссылку на проект `GoPaySDK` из вашего приложения:

```xml
<ProjectReference Include="..\GoPaySDK\GoPaySDK.csproj" />
```

Или выполните команду из папки вашего приложения:

```powershell
dotnet add reference ..\GoPaySDK\GoPaySDK.csproj
```

После этого можно подключать namespace:

```csharp
using GoPaySDK;
using GoPaySDK.Interfaces;
using GoPaySDK.Models;
```

## Конфигурация

SDK читает настройки из секции `GoPay`.

### appsettings.json

```json
{
  "GoPay": {
    "ApiKey": "YOUR_API_KEY",
    "SecretKey": "YOUR_SECRET_KEY",
    "BaseUrl": "https://api.example.com/"
  }
}
```

> Не храните реальные ключи в репозитории. Для локальной разработки используйте User Secrets или переменные окружения.

### User Secrets

```powershell
dotnet user-secrets set "GoPay:ApiKey" "YOUR_API_KEY"
dotnet user-secrets set "GoPay:SecretKey" "YOUR_SECRET_KEY"
dotnet user-secrets set "GoPay:BaseUrl" "https://api.example.com/"
```

## Использование в ASP.NET Core

### 1. Подключите сервис в `Program.cs`

```csharp
using GoPaySDK;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddGoPayService(builder.Configuration);

var app = builder.Build();

app.MapControllers();

app.Run();
```

### 2. Создание платежа в контроллере

```csharp
using GoPaySDK;
using GoPaySDK.Interfaces;
using GoPaySDK.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IGoPayService _goPayService;

    public PaymentsController(IGoPayService goPayService)
    {
        _goPayService = goPayService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment()
    {
        var result = await _goPayService.CreatePaymentAsync(new CreatePayment
        {
            order_id = Guid.CreateVersion7().AdoptToGoPay(),
            amount = 100,
            description = "Test payment",
            testing_mode = true,
            lifetime = 3600,
            callback_url = "https://your-domain.com/api/payments/callback",
            success_url = "https://your-domain.com/payment/success",
            failure_url = "https://your-domain.com/payment/failure"
        });

        if (result.status != ResponseMessages.StatusOK)
        {
            return BadRequest(result.error_message);
        }

        return Ok(result.data);
    }
}
```

### 3. Проверка платежа

```csharp
[HttpGet("{orderId}")]
public async Task<IActionResult> QueryPayment(string orderId)
{
    var result = await _goPayService.QueryPaymentAsync(new QueryPayment
    {
        order_id = orderId
    });

    if (result.status != ResponseMessages.StatusOK)
    {
        return BadRequest(result.error_message);
    }

    return Ok(result.data);
}
```

### 4. Webhook / Callback endpoint

`callback_url` — это Webhook endpoint вашего приложения. GoPay отправляет на этот URL уведомление с результатом платежа.

Этот endpoint должен быть доступен из интернета и принимать `POST`-запросы.

```csharp
[HttpPost("callback")]
public IActionResult Callback([FromBody] CallbackEvent callback)
{
    if (callback.status == Status.COMMITTED)
    {
        // Платеж успешно оплачен
    }

    return Ok();
}
```

## Использование в Console-приложении

### 1. Подключите Host и DI

```csharp
using GoPaySDK;
using GoPaySDK.Interfaces;
using GoPaySDK.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        if (context.HostingEnvironment.IsDevelopment())
        {
            config.AddUserSecrets<Program>();
        }
    })
    .ConfigureServices((context, services) =>
    {
        services.AddGoPayService(context.Configuration);
    })
    .Build();

using var scope = host.Services.CreateScope();
var goPayService = scope.ServiceProvider.GetRequiredService<IGoPayService>();
```

### 2. Создайте платеж

```csharp
var paymentResult = await goPayService.CreatePaymentAsync(new CreatePayment
{
    order_id = Guid.CreateVersion7().AdoptToGoPay(),
    amount = 1,
    description = "Test Payment",
    testing_mode = true
});

if (paymentResult.status == ResponseMessages.StatusOK)
{
    var payment = paymentResult.data;

    Console.WriteLine($"Payment ID: {payment?.payment_id}");
    Console.WriteLine($"Checkout URL: {payment?.checkout_url}");
    Console.WriteLine($"QR URL: {payment?.qr_url}");
}
else
{
    Console.WriteLine($"Error: {paymentResult.error_message}");
}
```

### 3. Проверьте статус платежа

```csharp
var queryResult = await goPayService.QueryPaymentAsync(new QueryPayment
{
    order_id = "ORDER_ID_WITHOUT_DASHES"
});

if (queryResult.status == ResponseMessages.StatusOK)
{
    Console.WriteLine($"Payment status: {queryResult.data?.status}");
}
else
{
    Console.WriteLine($"Error: {queryResult.error_message}");
}
```

## Модели

### CreatePayment

| Поле | Описание |
| --- | --- |
| `order_id` | ID заказа в системе мерчанта, максимум 32 символа |
| `amount` | Сумма платежа |
| `description` | Описание платежа, максимум 255 символов |
| `testing_mode` | Тестовый режим |
| `lifetime` | Время жизни платежа в секундах |
| `callback_url` | URL для callback-уведомления |
| `success_url` | URL редиректа после успешной оплаты |
| `failure_url` | URL редиректа после неуспешной оплаты |

### QueryPayment

| Поле | Описание |
| --- | --- |
| `payment_id` | ID платежа в GoPay |
| `order_id` | ID заказа в системе мерчанта |

## Статусы платежа

SDK содержит константы статусов:

```csharp
Status.CREATED
Status.PENDING
Status.FAILED
Status.COMMITTED
Status.EXPIRED
```

## Пример полного Console-приложения

Готовый пример находится в проекте:

```text
GoPayTest/Program.cs
```

## Безопасность

- Не коммитьте `ApiKey` и `SecretKey` в GitHub.
- Используйте HTTPS для `callback_url`, `success_url` и `failure_url`.
- Для production-окружения храните секреты в переменных окружения, Secret Manager, Azure Key Vault или другом защищенном хранилище.

## Лицензия

Этот проект распространяется под лицензией MIT. Подробнее см. файл `LICENSE`.
