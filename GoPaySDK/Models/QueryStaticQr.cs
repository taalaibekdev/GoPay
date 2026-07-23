namespace GoPaySDK.Models;

public class QueryStaticQr
{
    /// <summary>
    /// ID статического QR-кода в системе Go Pay.
    /// </summary>
    public required Guid qr_id { get; set; }
}
