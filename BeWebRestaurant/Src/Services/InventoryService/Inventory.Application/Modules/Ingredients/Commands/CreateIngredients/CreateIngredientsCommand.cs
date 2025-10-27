using Domain.Core.Interface.Request;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Requests.Ingredients;
using Inventory.Application.DTOs.Responses.Ingredients;
using Inventory.Domain.Common.Factories.Rule;
using MediatR;

namespace Inventory.Application.Modules.Ingredients.Commands.CreateIngredients
{
    public sealed record CreateIngredientsCommand(IngredientsRequest Request) : IRequest<IngredientsResponse>, IValidateRequest
    {
        public IEnumerable<IBusinessRule> GetRule()
        {
            yield return IngredientsRuleFactory.NameMaxLength(Request.IngredientsName);
            yield return IngredientsRuleFactory.NameNotEmpty(Request.IngredientsName);
            yield return DescriptionRuleFactory.DescriptionNotEmpty(Request.Description);
            yield return DescriptionRuleFactory.DescriptionMaxLength(Request.Description);
        }
    }
}