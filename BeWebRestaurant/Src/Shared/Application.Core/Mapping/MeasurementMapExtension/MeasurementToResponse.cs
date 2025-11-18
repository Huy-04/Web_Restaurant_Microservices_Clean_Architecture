using Application.Core.DTOs.Responses.Measurement;
using Domain.Core.ValueObjects;

namespace Application.Core.Mapping.MeasurementMapExtension

{
    public static class MeasurementToResponse
    {
        public static MeasurementResponse ToMeasurementResponse(this Measurement measurement)
        {
            return new(measurement.Quantity, measurement.Unit);
        }
    }
}