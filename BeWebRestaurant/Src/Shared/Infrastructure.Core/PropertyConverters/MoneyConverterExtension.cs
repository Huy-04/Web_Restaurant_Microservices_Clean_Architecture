using Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Infrastructure.Core.PropertyConverters
{
    public static class MoneyConverterExtension
    {
        public static readonly ValueConverter<Money, string> MoneyConverter = new(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => DeserializeMoney(v)
        );

        private static Money DeserializeMoney(string json)
        {
            var deserialized = JsonSerializer.Deserialize<Money>(json, (JsonSerializerOptions?)null)!;
            return Money.Create(deserialized.Amount, deserialized.Currency);
        }
    }
}
