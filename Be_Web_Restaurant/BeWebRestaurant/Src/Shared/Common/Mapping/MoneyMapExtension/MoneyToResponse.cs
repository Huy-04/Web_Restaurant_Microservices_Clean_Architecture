using Common.DTOs.Responses.Money;
using Domain.Core.ValueObjects;

namespace Common.Mapping.MoneyMapExtension
{
    public static class MoneyToResponse
    {
        public static MoneyResponse ToMoneyResponse(this Money money)
        {
            return new(money.Amount, money.Currency);
        }
    }
}