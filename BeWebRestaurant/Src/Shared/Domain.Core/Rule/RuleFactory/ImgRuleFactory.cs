using Domain.Core.Interface.Rule;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.StringRule;

namespace Domain.Core.Rule.RuleFactory
{
    public static class ImgRuleFactory
    {
        public static IBusinessRule ImgMaxLength(string value)
        {
            return new StringMaxLength(value, 255, CommonField.Img);
        }

        public static IBusinessRule ImgNotEmpty(string value)
        {
            return new StringNotEmpty(value, CommonField.Img);
        }
    }
}