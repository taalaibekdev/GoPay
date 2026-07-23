# GoPay SDK for .NET

GoPay SDK — библиотека для интеграции [Go Pay](https://www.gopay.kg/) в .NET-приложения. SDK помогает создавать платежи и проверять статус платежа через `IGoPayService`.

Официальная документация Go Pay для разработчиков доступна здесь: [https://doc.gopay.kg/v1/](https://doc.gopay.kg/v1/).

## Возможности

- Создание платежа в GoPay (динамический QR)
- Проверка платежа по `payment_id` или `order_id`
- Создание и запрос статических QR-кодов
- Получение списка платёжных приложений для deep links
- Поддержка контактных данных покупателя (buyer) для фискальных чеков
- Поддержка позиций чека (items) с НДС для поштучной отчётности
- Поддержка Dependency Injection
- Настройка через `IConfiguration`
- Подходит для ASP.NET Core и Console-приложений

## Требования

- .NET 10 или выше
- `ApiKey` и `SecretKey` от GoPay KG
- `BaseUrl` API GoPay

## Полезные ссылки

- Официальный сайт Go Pay: [https://www.gopay.kg/](https://www.gopay.kg/)
- Документация для разработчиков: [https://doc.gopay.kg/v1/](https://doc.gopay.kg/v1/)
- Личный кабинет мерчанта: [https://merchant.gopay.kg/](https://merchant.gopay.kg/)

## Установка

Установите пакет из NuGet:

```powershell
dotnet add package GoPaySDK
```

После этого можно подключать namespace:

```csharp
using GoPaySDK;
using GoPaySDK.Interfaces;
using GoPaySDK.Models;
```

### Использование через ProjectReference

Если вы хотите подключить SDK напрямую из исходного кода, добавьте ссылку на проект `GoPaySDK` из вашего приложения:

```xml
<ProjectReference Include="..\GoPaySDK\GoPaySDK.csproj" />
```

Или выполните команду из папки вашего приложения:

```powershell
dotnet add reference ..\GoPaySDK\GoPaySDK.csproj
```

## Конфигурация

SDK читает настройки из секции `GoPay`.

### appsettings.json

```json
{
  "GoPay": {
    "ApiKey": "YOUR_API_KEY",
    "SecretKey": "YOUR_SECRET_KEY",
    "BaseUrl": "YOUR_GOPAY_API_BASE_URL"
  }
}
```

Актуальный API URL и параметры интеграции смотрите в официальной документации Go Pay: [https://doc.gopay.kg/v1/](https://doc.gopay.kg/v1/).

> Не храните реальные ключи в репозитории. Для локальной разработки используйте User Secrets или переменные окружения.

### User Secrets

```powershell
dotnet user-secrets set "GoPay:ApiKey" "YOUR_API_KEY"
dotnet user-secrets set "GoPay:SecretKey" "YOUR_SECRET_KEY"
dotnet user-secrets set "GoPay:BaseUrl" "https://api.gopay.kg/"
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
            failure_url = "https://your-domain.com/payment/failure",
            // Опционально: контактные данные покупателя для фискального чека
            buyer = new BuyerInput
            {
                email = "customer@example.com",
                phone = "+996555123456"
            },
            // Опционально: позиции чека для поштучной отчётности
            items = new List<ItemInput>
            {
                new ItemInput
                {
                    name = "Чизкейк Нью-Йорк",
                    price = "500.00",
                    quantity = 2,
                    vat_rate = "12",
                    item_type = ItemTypeEnum.goods
                },
                new ItemInput
                {
                    name = "Доставка",
                    price = "200.00",
                    quantity = 1,
                    item_type = ItemTypeEnum.service
                }
            }
        });

        if (result.status != ResponseMessages.StatusOK)
        {
            return BadRequest(result.error_message);
        }

        // Возвращаем данные платежа включая deep links для банковских приложений
        var payment = result.data;
        return Ok(new
        {
            payment.payment_id,
            payment.order_id,
            payment.checkout_url,
            payment.qr_url,
            payment.qr_data,
            app_links = payment.app_links // Deep links: {"mbank": "mbank://...", "megapay": "megapay://..."}
        });
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
| `buyer` | Контактные данные покупателя (email/phone) для фискального чека |
| `items` | Позиции чека для поштучной отчётности |

### BuyerInput

| Поле | Описание |
| --- | --- |
| `email` | Email покупателя для отправки чека через ГНС |
| `phone` | Телефон покупателя в формате E.164 |

### ItemInput

| Поле | Описание |
| --- | --- |
| `name` | Наименование позиции, максимум 128 символов |
| `price` | Цена позиции |
| `quantity` | Количество |
| `vat_rate` | Ставка НДС (0-12, включая дробные) |
| `item_type` | Тип: `goods`, `service`, `work`, `other` |
| `code` | Код товара (ФФД тег 1162) для маркированных товаров |

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
GoPayExample/Program.cs
```

## Новые возможности в версии 1.1.0

### Статические QR-коды

Создание постоянного QR-кода для кассы:

```csharp
var qrResult = await _goPayService.CreateStaticQrAsync(new StaticQrInput
{
    name = "Касса №1",
    description = "Оплата кофе и напитков",
    amount = "250.00", // Опционально: фиксированная сумма
    callback_url = "https://merchant.example.com/static-qr/callback"
});

if (qrResult.status == ResponseMessages.StatusOK)
{
    var qr = qrResult.data;
    Console.WriteLine($"QR ID: {qr.qr_id}");
    Console.WriteLine($"QR URL: {qr.qr_url}");
    Console.WriteLine($"QR Data: {qr.qr_data}");
}
```

Запрос информации о статическом QR:

```csharp
var queryResult = await _goPayService.QueryStaticQrAsync(new QueryStaticQr
{
    qr_id = Guid.Parse("f1e2d3c4b5a6f1e2d3c4b5a6f1e2d3c4")
});
```

### Платёжные приложения (Deep Links)

Получение списка банковских приложений для открытия оплаты напрямую:

```csharp
var appsResult = await _goPayService.GetPaymentAppsAsync(new PaymentAppInput
{
    platform = PlatformEnum.Android // или iOS, any
});

if (appsResult.status == ResponseMessages.StatusOK)
{
    foreach (var app in appsResult.data)
    {
        // Замените {qr_data} на значение из платежа
        var deepLink = app.url.Replace("{qr_data}", paymentQrData);
        Console.WriteLine($"{app.name}: {deepLink}");
    }
}
```

### Вебхуки нового формата (events_url)

GoPay рекомендует использовать новый механизм вебхуков с явным типом события:

```csharp
[HttpPost("events")]
public IActionResult HandleEvent([FromBody] PaymentEventEnvelope envelope)
{
    switch (envelope.EventType)
    {
        case "payment.committed":
            // Платёж успешно оплачен
            var paymentId = envelope.data.payment_id;
            var amount = envelope.data.amount;
            break;
            
        case "payment.failed":
            // Платёж не выполнен
            break;
    }
    
    return Ok();
}
```

Для инвойсов и подписок:

```csharp
[HttpPost("invoice-events")]
public IActionResult HandleInvoiceEvent([FromBody] InvoiceEventEnvelope envelope)
{
    switch (envelope.EventType)
    {
        case "invoice.created":
            // Создан новый инвойс
            break;
            
        case "invoice.paid":
            // Инвойс оплачен
            var subscriptionStatus = envelope.data.subscription_status;
            break;
    }
    
    return Ok();
}
```

## Безопасность

- Не коммитьте `ApiKey` и `SecretKey` в GitHub.
- Используйте HTTPS для `callback_url`, `success_url` и `failure_url`.
- Для production-окружения храните секреты в переменных окружения, Secret Manager, Azure Key Vault или другом защищенном хранилище.

## Лицензия

Этот проект распространяется под лицензией MIT. Подробнее см. файл `LICENSE`.
