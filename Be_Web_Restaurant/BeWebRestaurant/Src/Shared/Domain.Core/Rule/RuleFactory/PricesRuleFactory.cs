using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.ListRule;
using Domain.Core.ValueObjects;

namespace Domain.Core.Rule.RuleFactory
{
    public static class PricesRuleFactory
    {
        public static IBusinessRule PricesNotEmpty(IEnumerable<Money> priceList)
        {
            return new ListNotEmpty<Money>(priceList, CommonField.Prices); ;
        }

        public static IBusinessRule PricesUniqueCurrency(IEnumerable<Money> priceList, string property = MoneyField.Currency)
        {
            return new IsUniqueProperty<Money>(priceList, CommonField.Prices, property);
        }
    }
}