using Domain.Core.Interface.Request;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Requests.FoodRecipe;
using Inventory.Application.DTOs.Responses.FoodRecipe;
using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Commands.UpdateFoodRecipe
{
    public sealed record UpdateFoodRecipeCommand(Guid IdFoodRecipe, FoodRecipeRequest Request) : IRequest<FoodRecipeResponse>, IValidateRequest
    {
        public IEnumerable<IBusinessRule> GetRule()
        {
            yield return MeasurementRuleFactory.QuantityNotNegative(Request.Measurement.Quantity);
            yield return MeasurementRuleFactory.UnitValidate(Request.Measurement.UnitEnum);
        }
    }
}