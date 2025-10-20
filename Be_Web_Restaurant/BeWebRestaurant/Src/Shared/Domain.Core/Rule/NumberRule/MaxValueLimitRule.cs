using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;

namespace Domain.Core.Rule.NumberRule
{
    public class MaxValueLimitRule<T> : IBusinessRule where T : struct, IComparable
    {
        private readonly T _value;
        private readonly T _maxValue;
        private readonly string _field;

        public MaxValueLimitRule(T value, T maxValue, string field)
        {
            _value = value;
            _maxValue = maxValue;
            _field = field;
        }

        public string Field => _field;

        public ErrorCode Error => ErrorCode.ExceedsMaximum;

        public IReadOnlyDictionary<string, object> Parameters => new Dictionary<string, object>
        {
            {ParamField.Value,$"{_value} > {_maxValue}" }
        };

        public bool IsSatisfied() => _value.CompareTo(_maxValue) <= 0;
    }
}