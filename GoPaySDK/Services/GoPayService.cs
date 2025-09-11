using GoPaySDK.Interfaces;
using GoPaySDK.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace GoPaySDK.Services;

public class GoPayService(IHttpClientFactory clientFactory) : IGoPayService
{
    private readonly HttpClient _httpClient = clientFactory.CreateClient(Variables.GoPay);

    public async Task<BaseResponse<PaymentData>> CreatePayment(CreatePayment payment)
    {
        try
        {
            var json = JsonConvert.SerializeObject(payment, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore
            });
            var nonce = Guid.NewGuid().ToString().Replace("-","");
            var payload = $"{nonce}\n{json}\n";

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(Variables.SecretKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var signature = BitConverter.ToString(hashBytes).Replace("-", "").ToUpperInvariant();

            var content = new StringContent(json, new MediaTypeHeaderValue("application/json"));

            using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments");
            request.Headers.Add("gopay-nonce", nonce);
            request.Headers.Add("gopay-signature", signature);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
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
            var json = JsonConvert.SerializeObject(query, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore
            });
            var nonce = Guid.NewGuid().ToString().Replace("-", "");
            var payload = $"{nonce}\n{json}\n";

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(Variables.SecretKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var signature = BitConverter.ToString(hashBytes).Replace("-", "").ToUpperInvariant();

            var content = new StringContent(json, new MediaTypeHeaderValue("application/json"));

            using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments/query");
            request.Headers.Add("gopay-nonce", nonce);
            request.Headers.Add("gopay-signature", signature);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
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
