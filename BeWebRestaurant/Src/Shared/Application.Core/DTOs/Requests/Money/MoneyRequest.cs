using Domain.Core.Enums;

namespace Application.Core.DTOs.Requests.Money
{
    public sealed record MoneyRequest(decimal Amount, CurrencyEnum CurrencyEnum)
    {
    }
}