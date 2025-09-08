using GoPaySDK.Interfaces;
using GoPaySDK.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace GoPaySDK.Services;

public class PaymentService():IPaymentService
{
    private HttpClient _httpClient = new();

    public async Task<BaseResponse<PaymentData>> CreatePayment(CreatePayment payment)
    {
        try
        {
            var dataStr = JsonConvert.SerializeObject(payment, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore
            });
            var nonce = Guid.NewGuid().ToString().Replace("-","");
            var payload = $"{nonce}\n{dataStr}\n";

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(Variables.GoPaySecretKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var signature = BitConverter.ToString(hashBytes).Replace("-", "").ToUpperInvariant();

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("gopay-api-key", Variables.GoPayApiKey);
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("gopay-nonce", nonce);
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("gopay-signature", signature);
            
            
            var content = new StringContent(dataStr, new MediaTypeHeaderValue("application/json"));
            var response = await _httpClient.PostAsync($"https://api.gopay.kg/v1/payments", content);
            response.EnsureSuccessStatusCode();
            var response_content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BaseResponse<PaymentData>>(response_content)
                ?? throw new Exception("Failed to deserialize response");

            return result;
        }
        catch (Exception ex)
        {
            return new("0001", "FAIL", ex.Message);
        }
    }

    public async Task<BaseResponse<PaymentData>> QueryPayment(QueryPayment query)
    {
        try
        {
            var dataStr = JsonConvert.SerializeObject(query, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore
            });
            var nonce = Guid.NewGuid().ToString().Replace("-", "");
            var payload = $"{nonce}\n{dataStr}\n";

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(Variables.GoPaySecretKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var signature = BitConverter.ToString(hashBytes).Replace("-", "").ToUpperInvariant();

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("gopay-api-key", Variables.GoPayApiKey);
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("gopay-nonce", nonce);
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("gopay-signature", signature);

            var content = new StringContent(dataStr, new MediaTypeHeaderValue("application/json"));
            var response = await _httpClient.PostAsync($"https://api.gopay.kg/v1/payments/query", content);
            response.EnsureSuccessStatusCode();
            var response_content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BaseResponse<PaymentData>>(response_content)
                ?? throw new Exception("Failed to deserialize response");

            return result;
        }
        catch (Exception ex)
        {
            return new("0001", "FAIL", ex.Message);
        }
    }
}
