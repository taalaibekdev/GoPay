namespace GoPaySDK.Models;

public class SubscriptionEventData
{
    /// <summary>
    /// ID подписки в GoPay billing.
    /// </summary>
    public required int subscription_id { get; set; }
    /// <summary>
    /// Новый статус подписки: active, paused, suspended, cancelled или completed.
    /// </summary>
    public required string status { get; set; }
    /// <summary>
    /// Предыдущий статус подписки (например, pending_start, past_due). Null для новых подписок.
    /// </summary>
    public string? previous_status { get; set; }
    /// <summary>
    /// ID оферты.
    /// </summary>
    public int? offer_id { get; set; }
    /// <summary>
    /// Название оферты.
    /// </summary>
    public string? offer_name { get; set; }
    /// <summary>
    /// Признак тестового платежа. true — тестовый; отсутствие поля означает боевой платёж.
    /// </summary>
    public bool? testing_mode { get; set; }
    /// <summary>
    /// Клиент подписки.
    /// </summary>
    public required EventClient client { get; set; }
    /// <summary>
    /// Дата следующего списания (YYYY-MM-DD). Null для завершённых/отменённых подписок.
    /// </summary>
    public string? next_billing_date { get; set; }
    /// <summary>
    /// Дата паузы (UTC ISO-8601 с Z). Только для subscription.paused.
    /// </summary>
    public DateTime? paused_at { get; set; }
    /// <summary>
    /// Дата отмены (UTC ISO-8601 с Z). Только для subscription.cancelled.
    /// </summary>
    public DateTime? cancelled_at { get; set; }
}