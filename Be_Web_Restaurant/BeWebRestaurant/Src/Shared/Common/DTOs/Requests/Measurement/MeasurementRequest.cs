using Domain.Core.Enums;

namespace Common.DTOs.Requests.Measurement
{
    public sealed record MeasurementRequest(decimal Quantity, UnitEnum UnitEnum)
    {
    }
}