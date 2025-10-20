using Domain.Core.Base;

namespace Domain.Core.ValueObjects
{
    public abstract class NameBase : ValueObject
    {
        public string Value { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        protected NameBase(string value)
        {
            Value = value;
        }

        public static implicit operator string(NameBase name) => name.Value;

        public override string ToString() => Value;
    }
}