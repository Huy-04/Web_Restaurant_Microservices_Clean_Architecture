namespace Inventory.Application.DTOs.Responses.Stock
{
    public sealed record StockResponse(
        Guid IdStock,
        string StockName,
        string Description,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt)
    {
    }
}