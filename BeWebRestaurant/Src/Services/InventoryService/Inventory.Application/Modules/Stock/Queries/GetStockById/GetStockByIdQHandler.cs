using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Responses.Stock;
using Inventory.Application.Interfaces;
using Inventory.Application.Mapping.StockMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;

namespace Inventory.Application.Modules.Stock.Queries.GetStockById
{
    public sealed class GetStockByIdQHandler : IRequestHandler<GetStockByIdQuery, StockResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetStockByIdQHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<StockResponse> Handle(GetStockByIdQuery query, CancellationToken token)
        {
            var stock = await _uow.StockRepo.GetByIdAsync(query.IdStock);
            if (stock is null)
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    StockField.IdStock,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                        {ParamField.Value,query.IdStock }
                    });
            }

            return stock.ToStockResponse();
        }
    }
}