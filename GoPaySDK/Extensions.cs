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
    public static string GetSignature(this string payload)
    {
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(Variables.SecretKey));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var signature = Convert.ToHexString(hashBytes).ToUpperInvariant();
        return signature;
    }
}
