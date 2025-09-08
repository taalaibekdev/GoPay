namespace GoPaySDK.Models;

public class BaseResponse<T>(string code = "0000", string status = "OK", string? error_message = null, T? data = null) where T : class
{
    /// <summary>
    /// 0000 - Success
    /// "0001" "0002" "0006" "0005" "0007" "0008" "0009" "0010" "0011" "0012" "0013" "0015" "0014" "0004" "9999"
    /// </summary>
    public string code { get; set; } = code;
    /// <summary>
    /// "OK" "FAIL"
    /// </summary>
    public string status { get; set; } = status;
    public string? error_message { get; set; } = error_message;

    public T? data { get; set; } = data;
}
