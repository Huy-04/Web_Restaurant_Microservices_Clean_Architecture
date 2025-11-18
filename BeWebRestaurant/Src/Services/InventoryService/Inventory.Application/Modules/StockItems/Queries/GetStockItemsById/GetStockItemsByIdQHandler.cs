using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Application.IUnitOfWork;
using Inventory.Application.Mapping.StockItemsMapExtension;
using Inventory.Application.Modules.StockItems.Queries.GetStockItemsById;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetStockItemsById
{
    public sealed class GetStockItemsByIdQHandler : IRequestHandler<GetStockItemsByIdQuery, StockItemsResponse>
    {
        private readonly IInventoryUnitOfWork _uow;

        public GetStockItemsByIdQHandler(IInventoryUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<StockItemsResponse> Handle(GetStockItemsByIdQuery query, CancellationToken token)
        {
            var stockItems = await _uow.StockItemsRepo.GetByIdAsync(query.IdStockItems, token);
            if (stockItems is null)
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    StockItemsField.IdStockItems,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                        {ParamField.Value,query.IdStockItems }
                    });
            }
            var stock = await _uow.StockRepo.GetByIdAsync(stockItems.StockId, token);
            var ingredients = await _uow.IngredientsRepo.GetByIdAsync(stockItems.IngredientsId, token);
            return stockItems.ToStockItemsResponse(stock!.StockName, ingredients!.IngredientsName);
        }
    }
}

