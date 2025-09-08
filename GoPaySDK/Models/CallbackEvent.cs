using System.Text.RegularExpressions;

namespace GoPaySDK.Models;

public class CallbackEvent
{
    public Guid payment_id { get; set; }
    public Guid order_id { get; set; }
    //[GeneratedRegex("^-?\\(?:\\.\\)?$")]
    public required string amount { get; set; }
    public required string status { get; set; }
    public DateTime created_at { get; set; }
    public DateTime committed_at { get; set; }
    public string? bank_op_date { get; set; }
}
