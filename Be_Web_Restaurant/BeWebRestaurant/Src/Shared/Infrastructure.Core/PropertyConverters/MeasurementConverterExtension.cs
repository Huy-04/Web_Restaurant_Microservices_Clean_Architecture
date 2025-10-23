using Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Infrastructure.Core.PropertyConverters
{
    public static class MeasurementConverterExtension
    {
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
