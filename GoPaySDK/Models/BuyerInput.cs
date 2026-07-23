using System.ComponentModel.DataAnnotations;

namespace GoPaySDK.Models;

public class BuyerInput
{
    /// <summary>
    /// Email покупателя для отправки фискального чека через ГНС (ФФД тег 1008).
    /// </summary>
    [EmailAddress]
    public string? email { get; set; }
    /// <summary>
    /// Телефон покупателя в формате E.164 для отправки фискального чека через ГНС (ФФД тег 1008). Максимальная длина 32 символа.
    /// </summary>
    [MaxLength(32)]
    [Phone]
    public string? phone { get; set; }
}
