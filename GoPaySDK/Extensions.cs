using System.Security.Cryptography;
using System.Text;

namespace GoPaySDK;

public static class Extensions
{
    public static string AdoptToGoPay(this Guid id)
    {
        return id.ToString().Replace("-", string.Empty);
    }
    public static string CreateNonce()
    {
        return Guid.CreateVersion7().AdoptToGoPay();
    }
    public static string GetSignature(string payload, string secretKey)
    {
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var signature = Convert.ToHexString(hashBytes).ToUpperInvariant();
        return signature;
    }

    /// <summary>
    /// Проверяет подпись вебхука GoPay (events_url и legacy callback_url).
    /// Алгоритм: HMAC-SHA512(webhook_secret, nonce + "\n" + rawBody + "\n") в верхнем регистре hex,
    /// сравнивается с заголовком GoPay-Signature.
    /// </summary>
    public static bool VerifyWebhookSignature(string nonce, string rawBody, string signature, string webhookSecret)
    {
        if (string.IsNullOrEmpty(nonce) || string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(webhookSecret))
        {
            return false;
        }

        var payload = $"{nonce}\n{rawBody}\n";
        var expected = GetSignature(payload, webhookSecret);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(expected),
            Encoding.UTF8.GetBytes(signature));
    }
}
