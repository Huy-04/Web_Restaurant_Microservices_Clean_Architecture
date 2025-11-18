using Application.Core.DTOs.Requests.Money;
using Domain.Core.ValueObjects;

namespace Application.Core.Mapping.MoneyMapExtension
{
    public static class RequestToMoney
    {
        public static Money ToMoney(this MoneyRequest request)
        {
            return Money.Create(request.Amount, request.CurrencyEnum);
        }
    }
}