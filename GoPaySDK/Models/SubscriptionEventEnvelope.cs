namespace GoPaySDK.Models;

public class SubscriptionEventEnvelope
{
    /// <summary>
    /// Тип события: subscription.activated, subscription.paused, subscription.suspended, subscription.cancelled или subscription.completed.
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
    public required SubscriptionEventData data { get; set; }
}