using Domain.Core.Enums;

namespace Application.Core.DTOs.Responses.Measurement
{
    public sealed record MeasurementResponse(decimal Quantity, UnitEnum UnitEnum)
    {
    }
}