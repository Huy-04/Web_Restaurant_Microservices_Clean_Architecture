using Domain.Core.Interface.Request;
using Domain.Core.Interface.Rule;
using MediatR;
using Menu.Application.DTOs.Requests.FoodType;
using Menu.Application.DTOs.Responses.FoodType;
using Menu.Domain.Common.Factories.Rules;

namespace Menu.Application.Modules.FoodTypes.Commands.UpdateFoodType
{
    public sealed record UpdateFoodTypeCommand(Guid IdFoodType, FoodTypeRequest Request)
        : IRequest<FoodTypeResponse>, IValidateRequest
    {
        public IEnumerable<IBusinessRule> GetRule()
        {
            yield return FoodTypeRuleFactory.NameMaxLength(Request.FoodTypeName);
            yield return FoodTypeRuleFactory.NameNotEmpty(Request.FoodTypeName);
        }
    }
}