using System.ComponentModel.DataAnnotations;

namespace GoPaySDK.Models;

public class ItemInput
{
    /// <summary>
    /// Наименование позиции чека. Максимальная длина 128 символов.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public required string name { get; set; }
    /// <summary>
    /// Цена позиции. Максимальное значение 999999.99. Минимальное значение 0.01.
    /// </summary>
    [Required]
    [RegularExpression(@"^\d{0,8}(?:\.\d{0,2})?$")]
    public required string price { get; set; }
    /// <summary>
    /// Количество позиций. Минимальное значение 1.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public required int quantity { get; set; }
    /// <summary>
    /// Ставка НДС (от 0 до 12, включая дробные значения).
    /// </summary>
    [RegularExpression(@"^\d{0,2}(?:\.\d{0,2})?$")]
    public string? vat_rate { get; set; }
    /// <summary>
    /// Тип позиции.
    /// </summary>
    public ItemTypeEnum? item_type { get; set; }
    /// <summary>
    /// Код товара (ФФД тег 1162) для маркированных товаров. Максимальная длина 64 символа.
    /// </summary>
    [MaxLength(64)]
    public string? code { get; set; }
}
