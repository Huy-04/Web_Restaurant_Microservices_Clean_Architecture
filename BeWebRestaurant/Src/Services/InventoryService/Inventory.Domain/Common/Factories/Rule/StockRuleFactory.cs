using Domain.Core.Interface.Rule;
using Domain.Core.Rule.StringRule;
using Inventory.Domain.Common.Messages.FieldNames;

namespace Inventory.Domain.Common.Factories.Rule
{
    public static class StockRuleFactory
    {
        public static IBusinessRule NameMaxLength(string value)
        {
            return new StringMaxLength(value, 50, StockField.StockName);
        }

        public static IBusinessRule NameNotEmpty(string value)
        {
            return new StringNotEmpty(value, StockField.StockName);
        }
    }
}