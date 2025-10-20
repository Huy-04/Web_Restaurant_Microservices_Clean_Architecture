using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.EnumRule;
using Domain.Core.Rule.NumberRule;

namespace Domain.Core.Rule.RuleFactory
{
    public static class MeasurementRuleFactory
    {
        public static IBusinessRule QuantityNotNegative(decimal quantity)
        {
            return new NotNegativeRule<decimal>(quantity, MeasurementField.Quantity);
        }

        public static IBusinessRule QuantityNotZero(decimal quantity)
        {
            return new NotZeroRule<decimal>(quantity, MeasurementField.Quantity);
        }

        public static IBusinessRule UnitValidate(UnitEnum unit)
        {
            var validate = Enum.GetValues(typeof(UnitEnum)).Cast<UnitEnum>().ToList();
            return new EnumValidateRule<UnitEnum>(unit, validate, MeasurementField.Unit);
        }
    }
}