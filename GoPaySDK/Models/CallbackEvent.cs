using System.Text.RegularExpressions;

namespace GoPaySDK.Models;

public class CallbackEvent
{
    /// <summary>
    /// ID платежа в системе Go Pay.
    /// </summary>
    public Guid payment_id { get; set; }
    /// <summary>
    /// ID заказа в системе мерчанта.
    /// </summary>
    public Guid order_id { get; set; }
    /// <summary>
    /// Сумма платежа.
    /// </summary>
    public required string amount { get; set; }
    /// <summary>
    /// Статус платежа: COMMITTED или FAILED.
    /// </summary>
    public required string status { get; set; }
    /// <summary>
    /// Признак тестового платежа. true — тестовый; отсутствие поля означает боевой платёж.
    /// </summary>
    public bool? testing_mode { get; set; }
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
