using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Application.Interfaces;
using Inventory.Application.Mapping.StockItemsMapExtension;
using Inventory.Application.Modules.StockItems.Queries.GetStockItemsById;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetById
{
    public sealed class GetIStockItemsByIdQHandler : IRequestHandler<GetStockItemsByIdQuery, StockItemsResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetIStockItemsByIdQHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<StockItemsResponse> Handle(GetStockItemsByIdQuery query, CancellationToken token)
        {
            var stockItems = await _uow.StockItemsRepo.GetByIdAsync(query.IdStockItems);
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
            var stock = await _uow.StockRepo.GetByIdAsync(stockItems.StockId);
            var ingredients = await _uow.IngredientsRepo.GetByIdAsync(stockItems.IngredientsId);
            return stockItems.ToStockItemsResponse(stock!.StockName, ingredients!.IngredientsName);
        }
    }
}