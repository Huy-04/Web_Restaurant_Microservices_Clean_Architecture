using Domain.Core.Interface.Request;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Requests.FoodRecipe;
using Inventory.Application.DTOs.Responses.FoodRecipe;
using Inventory.Domain.Common.Factories.Rule;
using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Commands.CreateFoodRecipe
{
    public sealed record CreateFoodRecipeCommand(FoodRecipeRequest Request) : IRequest<FoodRecipeResponse>, IValidateRequest
    {
        public IEnumerable<IBusinessRule> GetRule()
        {
            yield return MeasurementRuleFactory.QuantityNotNegative(Request.Measurement.Quantity);
            yield return MeasurementRuleFactory.UnitValidate(Request.Measurement.UnitEnum);
        }
    }
}