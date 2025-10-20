using Domain.Core.Enums;

namespace Common.DTOs.Requests.Money
{
    public sealed record MoneyRequest(decimal Amount, CurrencyEnum CurrencyEnum)
    {
    }
}