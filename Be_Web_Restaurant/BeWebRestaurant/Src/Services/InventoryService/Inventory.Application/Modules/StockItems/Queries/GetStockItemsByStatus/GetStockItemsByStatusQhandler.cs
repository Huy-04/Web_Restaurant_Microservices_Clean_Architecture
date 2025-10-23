using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Application.Interfaces;
using Inventory.Application.Mapping.StockItemsMapExtension;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetStockItemsByStatus
{
    public sealed class GetStockItemsByStatusQhandler : IRequestHandler<GetStockItemsByStatusQuery, IEnumerable<StockItemsResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetStockItemsByStatusQhandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<StockItemsResponse>> Handle(GetStockItemsByStatusQuery query, CancellationToken token)
        {
            var stockItemsList = await _uow.StockItemsRepo.GetByStatusAsync(query.StockItemsStatus);
            var stockList = await _uow.StockRepo.GetAllAsync();
            var ingredientsList = await _uow.IngredientsRepo.GetAllAsync(token);
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