using Domain.Core.Base;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.RuleFactory;

namespace Domain.Core.ValueObjects
{
    public sealed class Img : ValueObject
    {
        public string Value { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        private Img(string value)
        {
            Value = value;
        }

        public static Img Create(string value)
        {
            RuleValidator.CheckRules(new IBusinessRule[]
            {
                ImgRuleFactory.ImgMaxLength(value),
                ImgRuleFactory.ImgNotEmpty(value)
            });
            return new Img(value);
        }

        public static implicit operator string(Img img) => img.Value;

        public override string ToString() => Value;
    }
}