using Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Common.PropertyConverters
{
    public static class CommonConverters
    {
        public static readonly ValueConverter<Description, string>
            DescriptionConverter = new(v => v.Value, v => Description.Create(v));

        public static readonly ValueConverter<Img, string>
            ImgConverter = new(v => v.Value, v => Img.Create(v));

        public static readonly ValueConverter<Money, string> MoneyConverter = new(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => DeserializeMoney(v)
        );

        private static Money DeserializeMoney(string json)
        {
            var deserialized = JsonSerializer.Deserialize<Money>(json, (JsonSerializerOptions?)null)!;
            return Money.Create(deserialized.Amount, deserialized.Currency);
        }

        public static readonly ValueConverter<Measurement, string> MeasurementConverter = new(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => DeserializeMeasurement(v)
            );

        private static Measurement DeserializeMeasurement(string json)
        {
            var deserialized = JsonSerializer.Deserialize<Measurement>(json, (JsonSerializerOptions?)null)!;
            return Measurement.Create(deserialized.Quantity, deserialized.Unit);
        }
    }
}