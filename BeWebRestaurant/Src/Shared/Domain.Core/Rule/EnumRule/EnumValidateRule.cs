using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;

namespace Domain.Core.Rule.EnumRule
{
    public class EnumValidateRule<TEnum> : IBusinessRule where TEnum : Enum
    {
        private readonly TEnum _value;
        private readonly IEnumerable<TEnum> _validate;
        private readonly string _field;

        public EnumValidateRule(TEnum value, IEnumerable<TEnum> validValues, string field)
        {
            _value = value;
            _validate = validValues;
            _field = field;
        }

        public string Field => _field;

        public ErrorCode Error => ErrorCode.InvalidStatus;

        public IReadOnlyDictionary<string, object> Parameters => new Dictionary<string, object>();

        public bool IsSatisfied() => _validate.Contains(_value);
    }
}