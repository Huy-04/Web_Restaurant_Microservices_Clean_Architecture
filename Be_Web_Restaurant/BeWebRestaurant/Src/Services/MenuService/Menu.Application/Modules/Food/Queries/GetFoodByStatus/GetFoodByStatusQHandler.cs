using MediatR;
using Menu.Application.DTOs.Responses.Food;
using Menu.Application.Interfaces;
using Menu.Application.Mapping.FoodMapExtension;

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
            var lisFood = await _uow.FoodRepo.GetByStatusAsync(query.foodStatus);
            var listFoodType = await _uow.FoodTypeRepo.GetAllAsync();
            var list = from food in lisFood
                       join type in listFoodType on food.FoodTypeId equals type.Id
                       select food.ToFoodResponse(type.FoodTypeName);
            return list;
        }
    }
}