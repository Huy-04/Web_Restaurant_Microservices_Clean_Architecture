using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;

namespace Domain.Core.Rule.NumberRule
{
    public class NotNegativeRule<T> : IBusinessRule where T : struct, IComparable
    {
        private readonly T _value;
        private readonly string _field;

        public NotNegativeRule(T value, string field)
        {
            _value = value;
            _field = field;
        }

        public string Field => _field;

        public ErrorCode Error => ErrorCode.NotNegative;

        public IReadOnlyDictionary<string, object> Parameters => new Dictionary<string, object>
        {
            {ParamField.Value,_value }
        };

        public bool IsSatisfied() => _value.CompareTo(default(T)) >= 0;
    }
}