namespace GoPaySDK;

public class Variables
{
    public const string GoPay = "GoPay";
}

public class ResponseMessages
{
    public const string StatusOK = "OK";
    public const string StatusFAIL = "FAIL";
}
public class ResponseCodes
{
    public const string Success = "0000";
    public const string Fail = "0001";
    public const string TestRateLimitExceeded = "0004";
    public const string CancelImpossible = "0013";
    public const string BankUnavailableRetry = "0015";
    public const string OrganizationNotActive = "0016";
    public const string UnknownError = "9999";
}
public class Status
{
    public const string CREATED = "CREATED";
    public const string PENDING = "PENDING";
    public const string FAILED = "FAILED";
    public const string COMMITTED = "COMMITTED";
    public const string EXPIRED = "EXPIRED";
    public const string CANCELLED = "CANCELLED";
}
public class Events
{
    public const string PaymentCommitted = "payment.committed";
    public const string PaymentFailed = "payment.failed";
    public const string PaymentExpired = "payment.expired";
    public const string PaymentCancelled = "payment.cancelled";
    public const string ReceiptSent = "receipt.sent";
    public const string SubscriptionActivated = "subscription.activated";
    public const string SubscriptionPaused = "subscription.paused";
    public const string SubscriptionSuspended = "subscription.suspended";
    public const string SubscriptionCancelled = "subscription.cancelled";
    public const string SubscriptionCompleted = "subscription.completed";
    public const string InvoiceCreated = "invoice.created";
    public const string InvoicePaid = "invoice.paid";
    public const string TestPing = "test.ping";
}