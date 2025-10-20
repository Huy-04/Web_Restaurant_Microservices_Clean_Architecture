using Inventory.Application.DTOs.Responses.Stock;
using Inventory.Domain.Entities;

namespace Inventory.Application.Mapping.StockMapExtension
{
    public static class StockToResponse
    {
        public static StockResponse ToStockResponse(this Stock stock)
        {
            return new(
                stock.Id,
                stock.StockName,
                stock.Description,
                stock.CreatedAt,
                stock.UpdatedAt);
        }
    }
}