using System.Text.Json.Serialization;

namespace Menu.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FoodStatusEnum
    {
        Active = 1,
        Inactive = 2,
        Discontinued = 3,
    }
}