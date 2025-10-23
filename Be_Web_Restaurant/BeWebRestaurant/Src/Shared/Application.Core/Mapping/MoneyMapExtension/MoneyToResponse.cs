using Application.Core.DTOs.Responses.Money;
using Domain.Core.ValueObjects;

namespace Application.Core.Mapping.MoneyMapExtension
{
    public static class MoneyToResponse
    {
        public static MoneyResponse ToMoneyResponse(this Money money)
        {
            return new(money.Amount, money.Currency);
        }
    }
}