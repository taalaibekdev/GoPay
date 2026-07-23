namespace GoPaySDK.Models;

public class InvoiceEventData
{
    /// <summary>
    /// ID инвойса.
    /// </summary>
    public required int invoice_id { get; set; }
    /// <summary>
    /// Статус инвойса: pending (invoice.created) или paid (invoice.paid).
    /// </summary>
    public required string status { get; set; }
    /// <summary>
    /// Сумма инвойса.
    /// </summary>
    public required decimal amount { get; set; }
    /// <summary>
    /// Дата биллинга (YYYY-MM-DD).
    /// </summary>
    public required string billing_date { get; set; }
    /// <summary>
    /// Срок оплаты (YYYY-MM-DD).
    /// </summary>
    public required string due_date { get; set; }
    /// <summary>
    /// Дата оплаты (UTC ISO-8601 с Z) для invoice.paid.
    /// </summary>
    public DateTime? paid_at { get; set; }
    /// <summary>
    /// ID подписки.
    /// </summary>
    public int? subscription_id { get; set; }
    /// <summary>
    /// Статус подписки после оплаты.
    /// </summary>
    public string? subscription_status { get; set; }
    /// <summary>
    /// ID оферты.
    /// </summary>
    public int? offer_id { get; set; }
    /// <summary>
    /// Данные клиента.
    /// </summary>
    public required EventClient client { get; set; }
}
