using MediatR;
using Menu.Application.DTOs.Responses.Food;
using Menu.Application.Interfaces;
using Menu.Application.Mapping.FoodMapExtension;
using Menu.Domain.Enums;
using Menu.Domain.ValueObjects.Food;

namespace Menu.Application.Modules.Food.Queries.GetFoodByStatus
{
    public sealed class GetFoodByStatusQHandler : IRequestHandler<GetFoodByStatusQuery, IEnumerable<FoodResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetFoodByStatusQHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<FoodResponse>> Handle(GetFoodByStatusQuery query, CancellationToken token)
        {
            var raw = (query.Status ?? string.Empty).Trim();
            var enumStatus = Enum.Parse<FoodStatusEnum>(raw, ignoreCase: true);
            var foodStatus = FoodStatus.Create(enumStatus);

            var lisFood = await _uow.FoodRepo.GetByStatusAsync(foodStatus, token);
            var listFoodType = await _uow.FoodTypeRepo.GetAllAsync(token);
            var list = from food in lisFood
                       join type in listFoodType on food.FoodTypeId equals type.Id
                       select food.ToFoodResponse(type.FoodTypeName);
            return list;
        }
    }
}
