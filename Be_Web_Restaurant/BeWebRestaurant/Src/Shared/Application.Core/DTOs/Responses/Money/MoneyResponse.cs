using Domain.Core.Enums;

namespace Application.Core.DTOs.Responses.Money
{
    public sealed record MoneyResponse(decimal Amount, CurrencyEnum CurrencyEnum)
    {
    }
}