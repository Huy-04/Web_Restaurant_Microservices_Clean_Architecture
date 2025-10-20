using Common.DTOs.Responses.Measurement;
using Domain.Core.ValueObjects;

namespace Common.Mapping.MeasurementMapExtension
{
    public static class MeasurementToResponse
    {
        public static MeasurementResponse ToMeasurementResponse(this Measurement measurement)
        {
            return new(measurement.Quantity, measurement.Unit);
        }
    }
}