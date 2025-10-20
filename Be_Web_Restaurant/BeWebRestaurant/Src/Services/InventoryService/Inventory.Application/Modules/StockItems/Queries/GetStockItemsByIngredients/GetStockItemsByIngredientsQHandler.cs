using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Application.Interfaces;
using Inventory.Application.Mapping.StockItemsMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetStockItemsByIngredients
{
    public sealed class GetStockItemsByIngredientsQHandler : IRequestHandler<GetStockItemsByIngredientsQuery, IEnumerable<StockItemsResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetStockItemsByIngredientsQHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<StockItemsResponse>> Handle(GetStockItemsByIngredientsQuery query, CancellationToken token)
        {
            var stockItemsList = await _uow.StockItemsRepo.GetByIngredientsAsync(query.IngredientsId);
            if (!stockItemsList.Any())
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    IngredientsField.IdIngredients,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                        {ParamField.Value,query.IngredientsId }
                    });
            }
            var stockList = await _uow.StockRepo.GetAllAsync();
            var ingredientsList = await _uow.IngredientsRepo.GetAllAsync();
            var list = from si in stockItemsList
                       join s in stockList
                       on si.StockId equals s.Id
                       join i in ingredientsList
                       on si.IngredientsId equals i.Id
                       select si.ToStockItemsResponse(s.StockName, i.IngredientsName);
            return list;
        }
    }
}