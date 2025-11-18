using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Application.IUnitOfWork;
using Inventory.Application.Mapping.StockItemsMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetStockItemsByStock
{
    public sealed class GetStockItemsByStockQhandler : IRequestHandler<GetStockItemsByStockQuery, IEnumerable<StockItemsResponse>>
    {
        private readonly IInventoryUnitOfWork _uow;

        public GetStockItemsByStockQhandler(IInventoryUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<StockItemsResponse>> Handle(GetStockItemsByStockQuery query, CancellationToken token)
        {
            var stockItemsList = await _uow.StockItemsRepo.GetByStockAsync(query.stockId);
            if (!stockItemsList.Any())
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    StockField.IdStock,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                        {ParamField.Value,query.stockId }
                    });
            }
            var ingredientsList = await _uow.IngredientsRepo.GetAllAsync(token);
            var stockList = await _uow.StockRepo.GetAllAsync();
            var list = from si in stockItemsList
                       join i in ingredientsList on si.IngredientsId equals i.Id
                       join s in stockList on si.StockId equals s.Id
                       select si.ToStockItemsResponse(s.StockName, i.IngredientsName);
            return list;
        }
    }
}
