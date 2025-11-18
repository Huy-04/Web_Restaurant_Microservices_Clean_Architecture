using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.EnumRule;
using Domain.Core.Rule.NumberRule;

namespace Domain.Core.Rule.RuleFactory
{
    public static class MoneyRuleFactory
    {
        //factor
        public static IBusinessRule FactorNotNegative(decimal factor)
        {
            return new NotNegativeRule<decimal>(factor, MoneyField.Factor);
        }

        // amount
        public static IBusinessRule AmountNotNegative(decimal amount)
        {
            return new NotNegativeRule<decimal>(amount, MoneyField.Amount);
        }

        // Currency
        public static IBusinessRule CurrencyValidate(CurrencyEnum currency)
        {
            var validate = Enum.GetValues(typeof(CurrencyEnum)).Cast<CurrencyEnum>().ToList();
            return new EnumValidateRule<CurrencyEnum>(currency, validate, MoneyField.Currency);
        }

        public static IBusinessRule CurrencyEqual(CurrencyEnum left, CurrencyEnum right)
        {
            return new EnumEqualRule<CurrencyEnum>(left, right, MoneyField.Currency);
        }
    }
}