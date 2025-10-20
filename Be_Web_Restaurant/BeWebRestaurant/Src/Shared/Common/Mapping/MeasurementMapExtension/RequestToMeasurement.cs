using Common.DTOs.Requests.Measurement;
using Domain.Core.ValueObjects;

namespace Common.Mapping.MeasurementMapExtension
{
    public static class RequestToMeasurement
    {
        public static Measurement ToMeasurement(this MeasurementRequest request)
        {
            return Measurement.Create(request.Quantity, request.UnitEnum);
        }
    }
}