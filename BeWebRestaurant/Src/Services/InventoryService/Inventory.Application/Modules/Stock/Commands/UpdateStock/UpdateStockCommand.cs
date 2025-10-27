using Domain.Core.Interface.Request;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Requests.Stock;
using Inventory.Application.DTOs.Responses.Stock;
using Inventory.Domain.Common.Factories.Rule;
using MediatR;

namespace Inventory.Application.Modules.Stock.Commands.UpdateStock
{
    public sealed record UpdateStockCommand(Guid IdStock, StockRequest Request) : IRequest<StockResponse>, IValidateRequest
    {
        public IEnumerable<IBusinessRule> GetRule()
        {
            yield return StockRuleFactory.NameMaxLength(Request.StockName);
            yield return StockRuleFactory.NameNotEmpty(Request.StockName);
            yield return DescriptionRuleFactory.DescriptionNotEmpty(Request.Description);
            yield return DescriptionRuleFactory.DescriptionMaxLength(Request.Description);
        }
    }
}