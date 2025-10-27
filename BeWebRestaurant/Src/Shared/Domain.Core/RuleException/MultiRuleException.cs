namespace Domain.Core.RuleException
{
    public class MultiRuleException : Exception
    {
        public IEnumerable<BusinessRuleException> Errors { get; }

        public MultiRuleException(IEnumerable<BusinessRuleException> errors)
        {
            Errors = errors;
        }
    }
}