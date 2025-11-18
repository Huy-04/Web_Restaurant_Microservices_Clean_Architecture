using System.Text.Json.Serialization;

namespace Domain.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CurrencyEnum
    {
        VND = 1,
        USD = 2
    }
}