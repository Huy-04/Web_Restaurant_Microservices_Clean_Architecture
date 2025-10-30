using MediatR;
using Menu.Application.DTOs.Responses.FoodType;
using Menu.Application.Interface;
using Menu.Application.Mapping.FoodTypeMapExtension;

namespace Menu.Application.Modules.FoodType.Queries.GetAllFoodType
{
    public sealed class GetAllFoodTypeQHandler : IRequestHandler<GetAllFoodTypeQuery, IEnumerable<FoodTypeResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllFoodTypeQHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<FoodTypeResponse>> Handle(GetAllFoodTypeQuery query, CancellationToken token)
        {
            var list = await _uow.FoodTypeRepo.GetAllAsync(token);
            return list.Select(ft => ft.ToFoodTypeResponse());
        }
    }
}