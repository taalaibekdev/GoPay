namespace GoPaySDK.Models;

public class QueryPayment
{
    /// <summary>
    /// ID платежа в системе Go Pay.
    /// </summary>
    public required Guid payment_id { get; set; }
    /// <summary>
    /// ID заказа в системе мерчанта.Уникальный в системе мерчанта.Будет использован, если payment_id не указан.
    /// </summary>
    public required Guid order_id { get; set; }
}
