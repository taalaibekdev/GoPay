using GoPaySDK.Interfaces;
using GoPaySDK.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GoPaySDK.Services;

public class GoPayService : IGoPayService
{
    private readonly HttpClient _httpClient;
    private readonly GoPayOptions _options;
    private readonly JsonSerializerSettings _jsonSettings = new()
    {
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore
    };

    public GoPayService(HttpClient httpClient, IOptions<GoPayOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
            "gopay-api-key", _options.ApiKey);
    }

    public async Task<BaseResponse<PaymentData>> CreatePaymentAsync(CreatePayment payment)
    {
        try
        {
            var json = JsonConvert.SerializeObject(payment, _jsonSettings);
            var nonce = Extensions.CreateNonce();
            var payload = $"{nonce}\n{json}\n";
            var signature = Extensions.GetSignature(payload, _options.SecretKey);
            var content = new StringContent(json, new MediaTypeHeaderValue("application/json"));

            using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments");
            request.Headers.Add("gopay-nonce", nonce);
            request.Headers.Add("gopay-signature", signature);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var response_content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BaseResponse<PaymentData>>(response_content, _jsonSettings)
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
            var json = JsonConvert.SerializeObject(query, _jsonSettings);

            var nonce = Extensions.CreateNonce();
            var payload = $"{nonce}\n{json}\n";
            var signature = Extensions.GetSignature(payload, _options.SecretKey);
            var content = new StringContent(json, new MediaTypeHeaderValue("application/json"));

            using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments/query");
            request.Headers.Add("gopay-nonce", nonce);
            request.Headers.Add("gopay-signature", signature);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var response_content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BaseResponse<PaymentData>>(response_content, _jsonSettings)
                ?? throw new Exception("Failed to deserialize response");

            return result;
        }
        catch (Exception ex)
        {
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, ex.Message);
        }
    }
}
