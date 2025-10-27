using Domain.Core.Enums;
using Domain.Core.Interface.Rule;

namespace Domain.Core.Rule.StringRule
{
    public class StringNotEmpty : IBusinessRule
    {
        private readonly string _value;
        private readonly string _field;

        public StringNotEmpty(string value, string field)
        {
            _value = value;
            _field = field;
        }

        public bool IsSatisfied() => !string.IsNullOrEmpty(_value?.Trim());

        public string Field => _field;

        public ErrorCode Error => ErrorCode.NameEmpty;

        public IReadOnlyDictionary<string, object> Parameters => new Dictionary<string, object>();
    }
}