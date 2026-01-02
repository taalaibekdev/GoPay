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
    private readonly JsonSerializerSettings _options = new()
    {
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore
    };
    public async Task<BaseResponse<PaymentData>> CreatePaymentAsync(CreatePayment payment)
    {
        try
        {
            var json = JsonConvert.SerializeObject(payment, _options);
            var nonce = Extensions.CreateNonce();
            var payload = $"{nonce}\n{json}\n";
            var signature = payload.GetSignature();
            var content = new StringContent(json, new MediaTypeHeaderValue("application/json"));

            using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments");
            request.Headers.Add("gopay-nonce", nonce);
            request.Headers.Add("gopay-signature", signature);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var response_content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BaseResponse<PaymentData>>(response_content, _options)
                ?? throw new Exception("Failed to deserialize response");

            return result;
        }
        catch (Exception ex)
        {
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, ex.Message);
        }
    }

    public async Task<BaseResponse<PaymentData>> QueryPaymentAsync(QueryPayment query)
    {
        try
        {
            var json = JsonConvert.SerializeObject(query, _options);

            var nonce = Extensions.CreateNonce();
            var payload = $"{nonce}\n{json}\n";
            var signature = payload.GetSignature();
            var content = new StringContent(json, new MediaTypeHeaderValue("application/json"));

            using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments/query");
            request.Headers.Add("gopay-nonce", nonce);
            request.Headers.Add("gopay-signature", signature);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var response_content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BaseResponse<PaymentData>>(response_content, _options)
                ?? throw new Exception("Failed to deserialize response");

            return result;
        }
        catch (Exception ex)
        {
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, ex.Message);
        }
    }
}
