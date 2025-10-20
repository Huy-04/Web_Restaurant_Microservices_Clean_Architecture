using Domain.Core.Enums;

namespace Common.DTOs.Responses.Measurement
{
    public sealed record MeasurementResponse(decimal Quantity, UnitEnum UnitEnum)
    {
    }
}