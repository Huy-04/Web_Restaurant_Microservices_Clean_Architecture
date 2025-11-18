using MediatR;
using Menu.Application.DTOs.Responses.Food;
using Menu.Application.IUnitOfWork;
using Menu.Application.Mapping.FoodMapExtension;
using Menu.Domain.Enums;
using Menu.Domain.ValueObjects.Food;

namespace Menu.Application.Modules.Food.Queries.GetFoodByStatus
{
    public sealed class GetFoodByStatusQHandler : IRequestHandler<GetFoodByStatusQuery, IEnumerable<FoodResponse>>
    {
        private readonly IMenuUnitOfWork _uow;

        public GetFoodByStatusQHandler(IMenuUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<FoodResponse>> Handle(GetFoodByStatusQuery query, CancellationToken token)
        {
            var foodStatus = FoodStatus.Create(query.Status);

            var lisFood = await _uow.RFoodRepo.GetByStatusAsync(foodStatus, token);
            var listFoodType = await _uow.RFoodTypeRepo.GetAllAsync(token);
            var list = from food in lisFood
                       join type in listFoodType on food.FoodTypeId equals type.Id
                       select food.ToFoodResponse(type.FoodTypeName);
            return list;
        }
    }
}
