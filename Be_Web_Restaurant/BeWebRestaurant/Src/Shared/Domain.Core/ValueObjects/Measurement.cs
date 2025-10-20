using Domain.Core.Base;
using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule;
using Domain.Core.Rule.RuleFactory;
using System.Text.Json.Serialization;

namespace Domain.Core.ValueObjects
{
    public sealed class Measurement : ValueObject
    {
        public decimal Quantity { get; }

        public UnitEnum Unit { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Quantity;
            yield return Unit;
        }

        [JsonConstructor]
        private Measurement(decimal quantity, UnitEnum unit)
        {
            Quantity = quantity;
            Unit = unit;
        }

        public static Measurement Create(decimal quantity, UnitEnum unit)
        {
            RuleValidator.CheckRules(new IBusinessRule[]
            {
                MeasurementRuleFactory.QuantityNotNegative(quantity),
                MeasurementRuleFactory.UnitValidate(unit)
            });

            quantity = decimal.Round(quantity, 2, MidpointRounding.AwayFromZero);

            return new Measurement(quantity, unit);
        }

        public Measurement Add(Measurement other)
        {
            if (Unit != other.Unit)
            {
                other = other.ConvertTo(Unit);
            }
            return Create(Quantity + other.Quantity, Unit);
        }

        public Measurement Subtract(Measurement other)
        {
            if (Unit != other.Unit)
            {
                other = other.ConvertTo(Unit);
            }
            return Create(Quantity - other.Quantity, Unit);
        }

        public Measurement Divide(Measurement other)
        {
            RuleValidator.CheckRules(new IBusinessRule[]
            {
                MeasurementRuleFactory.QuantityNotZero(other.Quantity)
            });

            if (Unit != other.Unit)
            {
                other = other.ConvertTo(Unit);
            }

            return Create(Quantity / other.Quantity, Unit);
        }

        public static Measurement operator /(Measurement a, Measurement b) => a.Divide(b);

        public static Measurement operator +(Measurement a, Measurement b) => a.Add(b);

        public static Measurement operator -(Measurement a, Measurement b) => a.Subtract(b);

        public override string ToString() => $"{Quantity:0.##} {Unit}";

        private static readonly Dictionary<(UnitEnum from, UnitEnum to), decimal> ConversionRates =
            new Dictionary<(UnitEnum from, UnitEnum to), decimal>
            {
                // g-kg
                { (UnitEnum.g, UnitEnum.kg), 0.001m },
                { (UnitEnum.kg, UnitEnum.g), 1000m },

                // ml-l
                { (UnitEnum.ml, UnitEnum.l), 0.001m },
                { (UnitEnum.l, UnitEnum.ml), 1000m }
            };

        public Measurement ConvertTo(UnitEnum targetUnit)
        {
            if (Unit == targetUnit) return this;

            if (!ConversionRates.TryGetValue((Unit, targetUnit), out var factor))
            {
                throw RuleFactory.SimpleRuleException
                (
                    ErrorCategory.InternalServerError,
                    MeasurementField.Unit,
                    ErrorCode.TypeMismatch,
                    new Dictionary<string, object>
                    {
                        { ParamField.Value, $"{Unit} -> {targetUnit}" }
                    });
            }

            var newValue = Quantity * factor;
            return Create(newValue, targetUnit);
        }
    }
}