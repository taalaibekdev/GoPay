namespace GoPaySDK.Models;

public class EventClient
{
    /// <summary>
    /// ID клиента в GoPay billing.
    /// </summary>
    public required int id { get; set; }
    /// <summary>
    /// Имя клиента.
    /// </summary>
    public required string name { get; set; }
    /// <summary>
    /// Телефон клиента в формате E.164.
    /// </summary>
    public required string phone { get; set; }
    /// <summary>
    /// Email клиента.
    /// </summary>
    public string? email { get; set; }
}
