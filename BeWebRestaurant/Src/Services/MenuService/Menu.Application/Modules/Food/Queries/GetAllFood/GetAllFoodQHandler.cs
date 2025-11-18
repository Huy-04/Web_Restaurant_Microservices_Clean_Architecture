using MediatR;
using Menu.Application.DTOs.Responses.Food;
using Menu.Application.IUnitOfWork;
using Menu.Application.Mapping.FoodMapExtension;
using Menu.Application.Modules.Food.Queries.GetAllFood;

namespace Menu.Application.Modules.Food.Queries.GetAllFood
{
    public sealed class GetAllFoodQHandler : IRequestHandler<GetAllFoodQuery, IEnumerable<FoodResponse>>
    {
        private readonly IMenuUnitOfWork _uow;

        public GetAllFoodQHandler(IMenuUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<FoodResponse>> Handle(GetAllFoodQuery query, CancellationToken token)
        {
            var listFood = await _uow.RFoodRepo.GetAllAsync(token);
            var listFoodType = await _uow.RFoodTypeRepo.GetAllAsync(token);

            var list = from food in listFood
                       join type in listFoodType on food.FoodTypeId equals type.Id
                       select food.ToFoodResponse(type.FoodTypeName);

            return list;
        }
    }
}