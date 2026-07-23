namespace GoPaySDK.Models;

public class StaticQrData
{
    /// <summary>
    /// ID статического QR-кода в системе Go Pay.
    /// </summary>
    public required Guid qr_id { get; set; }
    /// <summary>
    /// Название QR-кода.
    /// </summary>
    public required string name { get; set; }
    /// <summary>
    /// Описание QR-кода.
    /// </summary>
    public string? description { get; set; }
    /// <summary>
    /// Фиксированная сумма платежа.
    /// </summary>
    public decimal? amount { get; set; }
    /// <summary>
    /// URL изображения QR-кода.
    /// </summary>
    public required string qr_url { get; set; }
    /// <summary>
    /// Данные EMVCO QR для самостоятельной отрисовки.
    /// </summary>
    public required string qr_data { get; set; }
    /// <summary>
    /// URL для уведомлений о платежах.
    /// </summary>
    public string? callback_url { get; set; }
    /// <summary>
    /// Дата создания QR-кода (UTC ISO-8601).
    /// </summary>
    public DateTime created_at { get; set; }
}
