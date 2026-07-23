namespace GoPaySDK.Models;

public class PaymentAppOutput
{
    /// <summary>
    /// Код платёжного приложения (например, mbank, megapay).
    /// </summary>
    public required string code { get; set; }
    /// <summary>
    /// Название платёжного приложения.
    /// </summary>
    public required string name { get; set; }
    /// <summary>
    /// Порядок отображения (основан на популярности).
    /// </summary>
    public required int order { get; set; }
    /// <summary>
    /// URL иконки приложения.
    /// </summary>
    public required string icon { get; set; }
    /// <summary>
    /// Шаблон deep link для запуска приложения. Замените {qr_data} на значение qr_data платежа.
    /// </summary>
    public required string url { get; set; }
    /// <summary>
    /// Имя пакета Android приложения в Google Play Store.
    /// </summary>
    public required string android_package_name { get; set; }
    /// <summary>
    /// Поддерживается ли приложение на iOS.
    /// </summary>
    public required bool is_active_on_ios { get; set; }
}
