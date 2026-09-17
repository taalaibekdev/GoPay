using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoPaySDK.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum ItemTypeEnum
{
    goods,
    service,
    work,
    other
}