using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;

namespace Domain.Core.Rule.StringRule
{
    public class StringMaxLength : IBusinessRule
    {
        private readonly string _value;
        private readonly string _field;
        private readonly int _maxlength;

        public StringMaxLength(string value, int maxlength, string field)
        {
            _value = value;
            _field = field;
            _maxlength = maxlength;
        }

        public bool IsSatisfied() => _value?.Trim().Length <= _maxlength;

        public string Field => _field;

        public ErrorCode Error => ErrorCode.NameTooLong;

        public IReadOnlyDictionary<string, object> Parameters
            => new Dictionary<string, object>
            {
                {ParamField.MaxLength,_maxlength },
            };
    }
}