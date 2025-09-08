namespace GoPaySDK.Models;

public class PaymentData
{
    /// <summary>
    /// ID платежа в системе Go Pay.
    /// </summary>
    public required Guid payment_id { get; set; }
    /// <summary>
    /// ID заказа в системе мерчанта.Уникальный в системе мерчанта.
    /// </summary>
    public required Guid order_id { get; set; }
    /// <summary>
    /// Сумма платежа
    /// </summary>
    public required decimal amount { get; set; }
    /// <summary>
    /// Статус платежа
    /// </summary>
    public required string status { get; set; }
    /// <summary>
    /// Описание платежа.
    /// </summary>
    public string? description { get; set; }
    /// <summary>
    /// List of payment application links.
    /// </summary>
    public object? app_links { get; set; }
    /// <summary>
    /// URL страницы оплаты.
    /// </summary>
    public required string checkout_url { get; set; }
    /// <summary>
    /// URL, на который будет отправлен результат платежа.
    /// </summary>
    public string? callback_url { get; set; }
    /// <summary>
    /// URL, на который пользователь будет перенаправлен после успешного платежа.
    /// </summary>
    public string? success_url { get; set; }
    /// <summary>
    /// URL, на который пользователь будет перенаправлен после неудачного платежа.
    /// </summary>
    public string? failure_url { get; set; }
    /// <summary>
    /// URL изображения QR кода.
    /// </summary>
    public required string qr_url { get; set; }
    /// <summary>
    /// Дата создания платежа.
    /// </summary>
    public DateTime created_at { get; set; }
    /// <summary>
    /// Срок платежа.
    /// </summary>
    public DateTime expires_at { get; set; }
    /// <summary>
    /// Дата оплаты платежа.
    /// </summary>
    public DateTime committed_at { get; set; }
    /// <summary>
    /// Дата банковского операционного дня, когда транзакция была обработана.
    /// </summary>
    public string? bank_op_date { get; set; }
}
