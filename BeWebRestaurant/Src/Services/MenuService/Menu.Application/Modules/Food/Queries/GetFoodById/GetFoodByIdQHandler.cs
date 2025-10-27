using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using MediatR;
using Menu.Application.DTOs.Responses.Food;
using Menu.Application.Interfaces;
using Menu.Application.Mapping.FoodMapExtension;
using Menu.Domain.Common.Messages.FieldNames;

namespace Menu.Application.Modules.Food.Queries.GetFoodById
{
    public sealed class GetFoodByIdQHandler : IRequestHandler<GetFoodByIdQuery, FoodResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetFoodByIdQHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<FoodResponse> Handle(GetFoodByIdQuery query, CancellationToken token)
        {
            var food = await _uow.FoodRepo.GetByIdAsync(query.IdFood);
            if (food is null)
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    FoodField.IdFood,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                        {ParamField.Value,query.IdFood }
                    });
            }
            var foodType = await _uow.FoodTypeRepo.GetByIdAsync(food.FoodTypeId);
            if (foodType is null)
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    FoodTypeField.IdFoodType,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                        {ParamField.Value, food.FoodTypeId }
                    });
            }
            return food.ToFoodResponse(foodType.FoodTypeName);
        }
    }
}