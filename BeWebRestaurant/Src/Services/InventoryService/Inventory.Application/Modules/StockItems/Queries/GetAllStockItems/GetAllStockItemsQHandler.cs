using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Application.IUnitOfWork;
using Inventory.Application.Mapping.StockItemsMapExtension;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetAllStockItems
{
    public sealed class GetAllStockItemsQHandler : IRequestHandler<GetAllStockItemsQuery, IEnumerable<StockItemsResponse>>
    {
        private readonly IInventoryUnitOfWork _uow;

        public GetAllStockItemsQHandler(IInventoryUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<StockItemsResponse>> Handle(GetAllStockItemsQuery query, CancellationToken token)
        {
            var stockItemsList = await _uow.StockItemsRepo.GetAllAsync(token);
            var stockList = await _uow.StockRepo.GetAllAsync(token);
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

