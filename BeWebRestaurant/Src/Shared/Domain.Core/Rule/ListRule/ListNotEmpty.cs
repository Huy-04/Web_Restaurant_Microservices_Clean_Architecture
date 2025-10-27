using Domain.Core.Enums;
using Domain.Core.Interface.Rule;

namespace Domain.Core.Rule.ListRule
{
    public class ListNotEmpty<T> : IBusinessRule
    {
        private readonly IEnumerable<T> _list;
        private readonly string _field;

        public ListNotEmpty(IEnumerable<T> list, string field)
        {
            _list = list;
            _field = field;
        }

        public string Field => _field;

        public ErrorCode Error => ErrorCode.CollectionEmpty;

        public IReadOnlyDictionary<string, object> Parameters => new Dictionary<string, object>();

        public bool IsSatisfied() => _list != null && _list.Any();
    }
}