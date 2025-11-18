using Domain.Core.Interface.Rule;
using Domain.Core.Rule.StringRule;
using Inventory.Domain.Common.Messages.FieldNames;

namespace Inventory.Domain.Common.Factories.Rule
{
    public static class IngredientsRuleFactory
    {
        public static IBusinessRule NameMaxLength(string name)
        {
            return new StringMaxLength(name, 50, IngredientsField.IngredientsName);
        }

        public static IBusinessRule NameNotEmpty(string name)
        {
            return new StringNotEmpty(name, IngredientsField.IngredientsName);
        }
    }
}