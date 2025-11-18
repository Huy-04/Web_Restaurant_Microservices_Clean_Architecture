using System.Text.Json.Serialization;

namespace Domain.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UnitEnum
    {
        kg = 1,
        g = 2,
        l = 3,
        ml = 4,
        piece = 5
    }
}