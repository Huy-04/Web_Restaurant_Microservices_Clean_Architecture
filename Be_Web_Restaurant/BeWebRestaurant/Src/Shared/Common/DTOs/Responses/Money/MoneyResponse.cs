using Domain.Core.Enums;

namespace Common.DTOs.Responses.Money
{
    public sealed record MoneyResponse(decimal Amount, CurrencyEnum CurrencyEnum)
    {
    }
}