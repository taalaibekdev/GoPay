namespace GoPaySDK.Models;

public class InvoiceEventEnvelope
{
    /// <summary>
    /// Тип события: invoice.created или invoice.paid.
    /// </summary>
    [Newtonsoft.Json.JsonProperty("event")]
    public required string EventType { get; set; }
    /// <summary>
    /// Время формирования события на стороне GoPay (UTC ISO-8601 с Z).
    /// </summary>
    public DateTime created_at { get; set; }
    /// <summary>
    /// Данные события.
    /// </summary>
    public required InvoiceEventData data { get; set; }
}
