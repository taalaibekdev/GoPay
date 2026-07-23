using System.ComponentModel.DataAnnotations;

namespace GoPaySDK.Models;

public class CreatePayment
{
    /// <summary>
    /// ID заказа в системе мерчанта. Максимальная длина 32 символа.
    /// </summary>
    [Required]
    [MaxLength(32)]
    public required string order_id { get; set; }
    /// <summary>
    /// Cумма платежа. Максимальное значение 999999.99. Минимальное значение 0.01.
    /// </summary>
    [Required]
    [Range(0.01, 999999.99)]
    public required decimal amount { get; set; }
    /// <summary>
    /// Описание платежа. Максимальная длина 255 символов.
    /// </summary>
    [MaxLength(255)]
    public required string? description { get; set; }
    /// <summary>
    /// Режим тестирования. Если True, платеж будет создан в режиме тестирования.
    /// </summary>
    public bool? testing_mode { get; set; } = false;
    /// <summary>
    /// Время жизни платежа в секундах. По умолчанию 3600 секунд. Минимальное значение 300 (5 минут) секунд. Максимальное значение 86400 секунд (24 часа).
    /// </summary>
    [Range(300, 86400)]
    public int? lifetime { get; set; } = 3600;
    /// <summary>
    /// URL, на который будет отправлен результат платежа. Разрешены только URL с протоколом 'https'. Если не указано, будет использован URL, указанный в настройках мерчанта.
    /// </summary>
    [MaxLength(2000)]
    [Url]
    public string? callback_url { get; set; }
    /// <summary>
    /// URL, на который пользователь будет перенаправлен после успешного платежа. Разрешены только URL с протоколом 'https'.
    /// </summary>
    [MaxLength(2000)]
    [Url]
    public string? success_url { get; set; }
    /// <summary>
    /// URL, на который пользователь будет перенаправлен после неудачного платежа. Разрешены только URL с протоколом 'https'.
    /// </summary>
    [MaxLength(2000)]
    [Url]
    public string? failure_url { get; set; }
    /// <summary>
    /// Контактные данные покупателя для отправки фискального чека через ГНС (ФФД тег 1008).
    /// </summary>
    public BuyerInput? buyer { get; set; }
    /// <summary>
    /// Позиции фискального чека. Обязательны для мерчантов, продающих товары под правила поштучной отчётности.
    /// </summary>
    public List<ItemInput>? items { get; set; }
}
