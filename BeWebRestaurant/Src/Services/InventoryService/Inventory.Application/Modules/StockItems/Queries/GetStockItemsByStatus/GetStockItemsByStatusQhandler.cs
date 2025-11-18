using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Application.IUnitOfWork;
using Inventory.Application.Mapping.StockItemsMapExtension;
using Inventory.Domain.Enums;
using Inventory.Domain.ValueObjects.StockItems;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetStockItemsByStatus
{
    public sealed class GetStockItemsByStatusQhandler : IRequestHandler<GetStockItemsByStatusQuery, IEnumerable<StockItemsResponse>>
    {
        private readonly IInventoryUnitOfWork _uow;

        public GetStockItemsByStatusQhandler(IInventoryUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<StockItemsResponse>> Handle(GetStockItemsByStatusQuery query, CancellationToken token)
        {
            var raw = (query.Status ?? string.Empty).Trim();
            var enumStatus = Enum.Parse<StockItemsStatusEnum>(raw, ignoreCase: true);
            var statusVo = StockItemsStatus.Create(enumStatus);

            var stockItemsList = await _uow.StockItemsRepo.GetByStatusAsync(statusVo, token);
            var stockList = await _uow.StockRepo.GetAllAsync(token);
            var ingredientsList = await _uow.IngredientsRepo.GetAllAsync(token);
            var list = from si in stockItemsList
                       join s in stockList on si.StockId equals s.Id
                       join i in ingredientsList on si.IngredientsId equals i.Id
                       select si.ToStockItemsResponse(s.StockName, i.IngredientsName);
            return list;
        }
    }
}

