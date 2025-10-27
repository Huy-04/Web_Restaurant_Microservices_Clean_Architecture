using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;

namespace Domain.Core.Rule.EnumRule
{
    public class EnumEqualRule<TEnum> : IBusinessRule where TEnum : Enum
    {
        private readonly TEnum _left;
        private readonly TEnum _right;
        private readonly string _field;

        public EnumEqualRule(TEnum left, TEnum right, string field)
        {
            _left = left;
            _right = right;
            _field = field;
        }

        public string Field => _field;

        public ErrorCode Error => ErrorCode.TypeMismatch;

        public IReadOnlyDictionary<string, object> Parameters => new Dictionary<string, object>
        {
            {ParamField.Value,$"{_left} != {_right}" }
        };

        public bool IsSatisfied() => _left.Equals(_right);
    }
}