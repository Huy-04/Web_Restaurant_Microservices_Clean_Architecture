using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Responses.Ingredients;
using Inventory.Application.IUnitOfWork;
using Inventory.Application.Mapping.IngredientsMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;

namespace Inventory.Application.Modules.Ingredients.Queries.GetIngredientsById
{
    public sealed class GetIngredientsByIdQHandler : IRequestHandler<GetIngredientsByIdQuery, IngredientsResponse>
    {
        private readonly IInventoryUnitOfWork _uow;

        public GetIngredientsByIdQHandler(IInventoryUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IngredientsResponse> Handle(GetIngredientsByIdQuery query, CancellationToken token)
        {
            var ingredients = await _uow.IngredientsRepo.GetByIdAsync(query.IdIngredients);
            if (ingredients is null)
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    IngredientsField.IdIngredients,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                        {ParamField.Value,query.IdIngredients }
                    });
            }

            return ingredients.ToIngredientsResponse();
        }
    }
}
