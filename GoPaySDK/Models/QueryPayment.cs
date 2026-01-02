namespace GoPaySDK.Models;

public class QueryPayment
{
    /// <summary>
    /// ID платежа в системе Go Pay(string <= 32 characters).
    /// </summary>
    public string? payment_id { get; set; }
    /// <summary>
    /// ID заказа в системе мерчанта.Уникальный в системе мерчанта.Будет использован, если payment_id не указан(string <= 32 characters).
    /// </summary>
    public string? order_id { get; set; }
}
