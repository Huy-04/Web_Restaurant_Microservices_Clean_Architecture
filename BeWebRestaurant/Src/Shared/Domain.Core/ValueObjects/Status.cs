using Domain.Core.Base;

namespace Domain.Core.ValueObjects
{
    public abstract class Status<TEnum> : ValueObject where TEnum : Enum
    {
        public TEnum Value { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        protected Status(TEnum value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();
    }
}