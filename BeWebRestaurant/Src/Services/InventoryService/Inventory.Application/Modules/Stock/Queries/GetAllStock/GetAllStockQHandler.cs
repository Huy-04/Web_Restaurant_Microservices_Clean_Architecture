using Inventory.Application.DTOs.Responses.Stock;
using Inventory.Application.Interface;
using Inventory.Application.Mapping.StockMapExtension;
using MediatR;

namespace Inventory.Application.Modules.Stock.Queries.GetAllStock
{
    public sealed class GetAllStockQHandler : IRequestHandler<GetAllStockQuery, IEnumerable<StockResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllStockQHandler(IUnitOfWork uow)
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