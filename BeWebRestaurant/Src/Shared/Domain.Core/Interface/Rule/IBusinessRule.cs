using Domain.Core.Enums;

namespace Domain.Core.Interface.Rule
{
    public interface IBusinessRule
    {
        ErrorCode Error { get; }

        string Field { get; }

        IReadOnlyDictionary<string, object> Parameters { get; }

        bool IsSatisfied();
    }
}