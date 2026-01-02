using System.ComponentModel.DataAnnotations;

namespace GoPaySDK.Models;

public class CreatePayment
{
    /// <summary>
    /// ID заказа в системе мерчанта. Максимальная длина 32 символа.
    /// </summary>
    public required string order_id { get; set; }
    /// <summary>
    /// Cумма платежа. Максимальное значение 999999.99. Минимальное значение 0.01. ^-?\d{0,8}(?:\.\d{0,2})?$
    /// </summary>
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
    public int? lifetime { get; set; } = 3600;
    /// <summary>
    /// URL, на который будет отправлен результат платежа. Разрешены только URL с протоколом 'https'. Если не указано, будет использован URL, указанный в настройках мерчанта. Если значение указано, оно должно быть действительным URL, в противном случае не будет отправлено.
    /// </summary>
    [MaxLength(2000)]
    public string? callback_url { get; set; }
    /// <summary>
    /// URL, на который пользователь будет перенаправлен после успешного платежа. Разрешены только URL с протоколом 'https'. Если не указано, пользователь не будет перенаправлен никуда.
    /// </summary>
    [MaxLength(2000)]
    public string? success_url { get; set; }
    /// <summary>
    /// URL, на который пользователь будет перенаправлен после неудачного платежа. Разрешены только URL с протоколом 'https'. Если не указано, пользователь не будет перенаправлен никуда.
    /// </summary>
    [MaxLength(2000)]
    public string? failure_url { get; set; }
}
