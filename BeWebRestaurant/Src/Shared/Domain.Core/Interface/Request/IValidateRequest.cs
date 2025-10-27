using Domain.Core.Interface.Rule;

namespace Domain.Core.Interface.Request
{
    public interface IValidateRequest
    {
        public IEnumerable<IBusinessRule> GetRule();
    }
}