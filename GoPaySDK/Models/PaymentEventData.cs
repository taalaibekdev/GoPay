namespace GoPaySDK.Models;

public class PaymentEventData
{
    /// <summary>
    /// ID платежа в системе Go Pay.
    /// </summary>
    public required Guid payment_id { get; set; }
    /// <summary>
    /// ID заказа в системе мерчанта.
    /// </summary>
    public required Guid order_id { get; set; }
    /// <summary>
    /// Сумма платежа.
    /// </summary>
    public required decimal amount { get; set; }
    /// <summary>
    /// Статус платежа: COMMITTED или FAILED.
    /// </summary>
    public required string status { get; set; }
    /// <summary>
    /// Дата создания платежа (UTC ISO-8601 с Z).
    /// </summary>
    public DateTime created_at { get; set; }
    /// <summary>
    /// Дата оплаты платежа (UTC ISO-8601 с Z).
    /// </summary>
    public DateTime? committed_at { get; set; }
    /// <summary>
    /// Дата банковского операционного дня (UTC ISO-8601 с Z).
    /// </summary>
    public string? bank_op_date { get; set; }
    /// <summary>
    /// ID статического QR-кода (если платеж через статический QR).
    /// </summary>
    public Guid? qr_id { get; set; }
    /// <summary>
    /// Название статического QR-кода (если платеж через статический QR).
    /// </summary>
    public string? qr_name { get; set; }
}
