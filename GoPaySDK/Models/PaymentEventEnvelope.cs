namespace GoPaySDK.Models;

public class PaymentEventEnvelope
{
    /// <summary>
    /// Тип события: payment.committed или payment.failed.
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
    public required PaymentEventData data { get; set; }
}
