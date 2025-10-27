using Domain.Core.ValueObjects;
using Inventory.Application.DTOs.Requests.Stock;
using Inventory.Domain.Entities;
using Inventory.Domain.ValueObjects.Stock;

namespace Inventory.Application.Mapping.StockMapExtension
{
    public static class RequestToStock
    {
        public static Stock ToStock(this StockRequest request)
        {
            return Stock.Create(
                StockName.Create(request.StockName),
                Description.Create(request.Description)
                );
        }
    }
}