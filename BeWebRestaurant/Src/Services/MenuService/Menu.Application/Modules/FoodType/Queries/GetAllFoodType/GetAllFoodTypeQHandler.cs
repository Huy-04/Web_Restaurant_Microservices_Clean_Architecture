using MediatR;
using Menu.Application.DTOs.Responses.FoodType;
using Menu.Application.IUnitOfWork;
using Menu.Application.Mapping.FoodTypeMapExtension;

namespace Menu.Application.Modules.FoodType.Queries.GetAllFoodType
{
    public sealed class GetAllFoodTypeQHandler : IRequestHandler<GetAllFoodTypeQuery, IEnumerable<FoodTypeResponse>>
    {
        private readonly IMenuUnitOfWork _uow;

        public GetAllFoodTypeQHandler(IMenuUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<FoodTypeResponse>> Handle(GetAllFoodTypeQuery query, CancellationToken token)
        {
            var list = await _uow.RFoodTypeRepo.GetAllAsync(token);
            return list.Select(ft => ft.ToFoodTypeResponse());
        }
    }
}