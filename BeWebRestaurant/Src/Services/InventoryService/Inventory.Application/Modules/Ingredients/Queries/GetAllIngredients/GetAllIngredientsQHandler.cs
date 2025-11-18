using Inventory.Application.DTOs.Responses.Ingredients;
using Inventory.Application.IUnitOfWork;
using Inventory.Application.Mapping.IngredientsMapExtension;
using Inventory.Application.Modules.Ingredients.Queries.GetAllIngredients;
using MediatR;

namespace Inventory.Application.Modules.Ingredients.Queries.GetAllIngredients
{
    public sealed class GetAllIngredientsQHandler : IRequestHandler<GetAllIngredientsQuery, IEnumerable<IngredientsResponse>>
    {
        private readonly IInventoryUnitOfWork _uow;

        public GetAllIngredientsQHandler(IInventoryUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<IngredientsResponse>> Handle(GetAllIngredientsQuery query, CancellationToken token)
        {
            var ingredientsList = await _uow.IngredientsRepo.GetAllAsync(token);

            return ingredientsList.Select(i => i.ToIngredientsResponse());
        }
    }
}
