using GoPaySDK.Models;

namespace GoPaySDK.Interfaces;

public interface IGoPayService
{
    Task<BaseResponse<PaymentData>> CreatePaymentAsync(CreatePayment payment);
    Task<BaseResponse<PaymentData>> QueryPaymentAsync(QueryPayment query);
}
