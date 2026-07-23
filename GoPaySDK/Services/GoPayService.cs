using GoPaySDK.Interfaces;
using GoPaySDK.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GoPaySDK.Services;

public class GoPayService : IGoPayService
{
    private readonly HttpClient _httpClient;
    private readonly GoPayOptions _options;
    private readonly ILogger<GoPayService> _logger;
    private readonly JsonSerializerSettings _jsonSettings = new()
    {
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore
    };

    public GoPayService(HttpClient httpClient, IOptions<GoPayOptions> options, ILogger<GoPayService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add(
            "gopay-api-key", _options.ApiKey);
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<BaseResponse<PaymentData>> CreatePaymentAsync(CreatePayment payment)
    {
        try
        {
            var json = JsonConvert.SerializeObject(payment, _jsonSettings);
            var nonce = Extensions.CreateNonce();
            var payload = $"{nonce}\n{json}\n";
            var signature = Extensions.GetSignature(payload, _options.SecretKey);

            _logger.LogDebug("Creating payment for order {OrderId}, amount {Amount}", payment.order_id, payment.amount);

            var content = new StringContent(json, new MediaTypeHeaderValue("application/json"));

            using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments");
            request.Headers.Add("gopay-nonce", nonce);
            request.Headers.Add("gopay-signature", signature);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var response_content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BaseResponse<PaymentData>>(response_content, _jsonSettings)
                ?? throw new JsonException("Failed to deserialize response");

            _logger.LogInformation("Payment created successfully: {PaymentId}", result.data?.payment_id);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while creating payment");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "GoPay API unavailable");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON error while processing payment response");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "Invalid response format");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating payment");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "Internal error");
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

            _logger.LogDebug("Querying payment: OrderId={OrderId}, PaymentId={PaymentId}", query.order_id, query.payment_id);

            var content = new StringContent(json, new MediaTypeHeaderValue("application/json"));

            using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments/query");
            request.Headers.Add("gopay-nonce", nonce);
            request.Headers.Add("gopay-signature", signature);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var response_content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BaseResponse<PaymentData>>(response_content, _jsonSettings)
                ?? throw new JsonException("Failed to deserialize response");

            if (result.status == ResponseMessages.StatusOK)
            {
                _logger.LogInformation("Payment queried successfully: {PaymentId}, Status={Status}", result.data?.payment_id, result.data?.status);
            }
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while querying payment");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "GoPay API unavailable");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON error while processing query response");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "Invalid response format");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while querying payment");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "Internal error");
        }
    }

    public async Task<BaseResponse<StaticQrData>> CreateStaticQrAsync(StaticQrInput qr)
    {
        try
        {
            var json = JsonConvert.SerializeObject(qr, _jsonSettings);
            var nonce = Extensions.CreateNonce();
            var payload = $"{nonce}\n{json}\n";
            var signature = Extensions.GetSignature(payload, _options.SecretKey);

            _logger.LogDebug("Creating static QR: {QrName}", qr.name);

            var content = new StringContent(json, new MediaTypeHeaderValue("application/json"));

            using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/static-qr/");
            request.Headers.Add("gopay-nonce", nonce);
            request.Headers.Add("gopay-signature", signature);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var response_content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BaseResponse<StaticQrData>>(response_content, _jsonSettings)
                ?? throw new JsonException("Failed to deserialize response");

            _logger.LogInformation("Static QR created successfully: {QrId}", result.data?.qr_id);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while creating static QR");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "GoPay API unavailable");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON error while processing QR response");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "Invalid response format");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating static QR");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "Internal error");
        }
    }

    public async Task<BaseResponse<StaticQrData>> QueryStaticQrAsync(QueryStaticQr query)
    {
        try
        {
            var json = JsonConvert.SerializeObject(query, _jsonSettings);
            var nonce = Extensions.CreateNonce();
            var payload = $"{nonce}\n{json}\n";
            var signature = Extensions.GetSignature(payload, _options.SecretKey);

            _logger.LogDebug("Querying static QR: {QrId}", query.qr_id);

            var content = new StringContent(json, new MediaTypeHeaderValue("application/json"));

            using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/static-qr/query");
            request.Headers.Add("gopay-nonce", nonce);
            request.Headers.Add("gopay-signature", signature);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var response_content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BaseResponse<StaticQrData>>(response_content, _jsonSettings)
                ?? throw new JsonException("Failed to deserialize response");

            _logger.LogInformation("Static QR queried successfully: {QrId}", result.data?.qr_id);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while querying static QR");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "GoPay API unavailable");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON error while processing QR query response");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "Invalid response format");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while querying static QR");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "Internal error");
        }
    }

    public async Task<BaseResponse<List<PaymentAppOutput>>> GetPaymentAppsAsync(PaymentAppInput input)
    {
        try
        {
            var json = JsonConvert.SerializeObject(input, _jsonSettings);
            var nonce = Extensions.CreateNonce();
            var payload = $"{nonce}\n{json}\n";
            var signature = Extensions.GetSignature(payload, _options.SecretKey);

            _logger.LogDebug("Getting payment apps for platform: {Platform}", input.platform);

            var content = new StringContent(json, new MediaTypeHeaderValue("application/json"));

            using var request = new HttpRequestMessage(HttpMethod.Post, $"v1/payment-app");
            request.Headers.Add("gopay-nonce", nonce);
            request.Headers.Add("gopay-signature", signature);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var response_content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BaseResponse<List<PaymentAppOutput>>>(response_content, _jsonSettings)
                ?? throw new JsonException("Failed to deserialize response");

            _logger.LogInformation("Retrieved {Count} payment apps", result.data?.Count);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while getting payment apps");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "GoPay API unavailable");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON error while processing apps response");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "Invalid response format");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while getting payment apps");
            return new(ResponseCodes.Fail, ResponseMessages.StatusFAIL, "Internal error");
        }
    }
}
