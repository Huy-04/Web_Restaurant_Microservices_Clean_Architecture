using Application.Core.DTOs.Requests.Measurement;
using Domain.Core.Interface.Request;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Responses.StockItems;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Commands.IncreaseStockItems
{
    public sealed record IncreaseStockItemCommand(Guid IdStockItems, MeasurementRequest Request) : IRequest<StockItemsResponse>, IValidateRequest
    {
        public IEnumerable<IBusinessRule> GetRule()
        {
            yield return MeasurementRuleFactory.QuantityNotNegative(Request.Quantity);
            yield return MeasurementRuleFactory.UnitValidate(Request.UnitEnum);
        }
    }
}