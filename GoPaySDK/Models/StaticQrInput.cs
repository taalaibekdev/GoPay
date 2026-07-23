using System.ComponentModel.DataAnnotations;

namespace GoPaySDK.Models;

public class StaticQrInput
{
    /// <summary>
    /// Название QR-кода. Максимальная длина 128 символов.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public required string name { get; set; }
    /// <summary>
    /// Описание QR-кода. Максимальная длина 255 символов.
    /// </summary>
    [MaxLength(255)]
    public string? description { get; set; }
    /// <summary>
    /// Фиксированная сумма платежа. Если не указана, покупатель может ввести любую сумму.
    /// </summary>
    [RegularExpression(@"^\d{0,8}(?:\.\d{0,2})?$")]
    public string? amount { get; set; }
    /// <summary>
    /// URL для уведомлений о платежах. Разрешены только HTTPS URL.
    /// </summary>
    [MaxLength(2000)]
    [Url]
    public string? callback_url { get; set; }
}
