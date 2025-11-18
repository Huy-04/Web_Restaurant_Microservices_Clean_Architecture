using Domain.Core.Base;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.RuleFactory;

namespace Domain.Core.ValueObjects
{
    public sealed class PriceList : ValueObject
    {
        private readonly List<Money> _items = new List<Money>();
        private readonly List<Money> _sortedItems;

        public IReadOnlyCollection<Money> Items => _items.AsReadOnly();

        protected override IEnumerable<object> GetAtomicValues() => _sortedItems;

        private PriceList(IEnumerable<Money> items)
        {
            _items.AddRange(items);
            _sortedItems = _items.OrderBy(x => x.Currency).ToList();
        }

        public static PriceList Create(IEnumerable<Money> items)
        {
            var list = items.ToList();

            RuleValidator.CheckRules(new IBusinessRule[]
            {
                PricesRuleFactory.PricesNotEmpty(list),

                PricesRuleFactory.PricesUniqueCurrency(list)
            });

            return new PriceList(list);
        }

        public PriceList AddOrReplace(Money price)
        {
            var clone = new List<Money>(_items);
            var index = clone.FindIndex(x => x.Currency == price.Currency);
            if (index >= 0)
            {
                clone[index] = price;
            }
            else
            {
                clone.Add(price);
            }

            return new PriceList(clone);
        }
    }
}