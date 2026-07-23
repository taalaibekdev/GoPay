namespace GoPaySDK.Models;

public class PaymentAppInput
{
    /// <summary>
    /// Платформа устройства: Android, iOS или any для всех платформ.
    /// </summary>
    public required PlatformEnum platform { get; set; }
}
