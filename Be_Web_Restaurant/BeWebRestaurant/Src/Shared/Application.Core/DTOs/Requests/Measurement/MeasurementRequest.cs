using Domain.Core.Enums;

namespace Application.Core.DTOs.Requests.Measurement
{
    public sealed record MeasurementRequest(decimal Quantity, UnitEnum UnitEnum)
    {
    }
}