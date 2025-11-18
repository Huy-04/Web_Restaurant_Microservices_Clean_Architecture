using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;

namespace Domain.Core.Rule.ListRule
{
    public class IsUniqueProperty<T> : IBusinessRule
    {
        private readonly IEnumerable<T> _list;
        private readonly string _field;
        private readonly string _property;

        public IsUniqueProperty(IEnumerable<T> list, string field, string property)
        {
            _list = list;
            _field = field;
            _property = property;
        }

        public ErrorCode Error => ErrorCode.DuplicateEntry;

        public string Field => _field;

        public IReadOnlyDictionary<string, object> Parameters => new Dictionary<string, object>
        {
            {ParamField.Value,_property ?? string.Empty}
        };

        public bool IsSatisfied()
        {
            var propertyInfo = typeof(T).GetProperty(_property);

            if (propertyInfo == null)
            {
                return false;
            }

            return _list.GroupBy(x => propertyInfo.GetValue(x)).All(g => g.Count() == 1);
        }
    }
}