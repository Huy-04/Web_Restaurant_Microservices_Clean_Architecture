using Inventory.Application.DTOs.Responses.Stock;
using Inventory.Application.IUnitOfWork;
using Inventory.Application.Mapping.StockMapExtension;
using MediatR;

namespace Inventory.Application.Modules.Stock.Queries.GetAllStock
{
    public sealed class GetAllStockQHandler : IRequestHandler<GetAllStockQuery, IEnumerable<StockResponse>>
    {
        private readonly IInventoryUnitOfWork _uow;

        public GetAllStockQHandler(IInventoryUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<StockResponse>> Handle(GetAllStockQuery query, CancellationToken token)
        {
            var stockList = await _uow.StockRepo.GetAllAsync(token);
            return stockList.Select(s => s.ToStockResponse());
        }
    }
}
