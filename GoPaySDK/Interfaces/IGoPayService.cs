using GoPaySDK.Models;

namespace GoPaySDK.Interfaces;

public interface IGoPayService
{
    Task<BaseResponse<PaymentData>> CreatePaymentAsync(CreatePayment payment);
    Task<BaseResponse<PaymentData>> QueryPaymentAsync(QueryPayment query);
    Task<BaseResponse<StaticQrData>> CreateStaticQrAsync(StaticQrInput qr);
    Task<BaseResponse<StaticQrData>> QueryStaticQrAsync(QueryStaticQr query);
    Task<BaseResponse<List<PaymentAppOutput>>> GetPaymentAppsAsync(PaymentAppInput input);
}
