using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using MediatR;
using Menu.Application.DTOs.Responses.Food;
using Menu.Application.IUnitOfWork;
using Menu.Application.Mapping.FoodMapExtension;
using Menu.Domain.Common.Messages.FieldNames;

namespace Menu.Application.Modules.Food.Queries.GetFoodByFoodType
{
    public sealed class GetFoodByFoodTypeQHandler : IRequestHandler<GetFoodByFoodTypeQuery, IEnumerable<FoodResponse>>
    {
        private readonly IMenuUnitOfWork _uow;

        public GetFoodByFoodTypeQHandler(IMenuUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<FoodResponse>> Handle(GetFoodByFoodTypeQuery query, CancellationToken token)
        {
            var listFood = await _uow.RFoodRepo.GetByFoodTypeAsync(query.FoodTypeId);
            if (!listFood.Any())
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    FoodTypeField.IdFoodType,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                        {ParamField.Value,query.FoodTypeId }
                    });
            }
            var listFoodType = await _uow.RFoodTypeRepo.GetAllAsync();
            var list = from f in listFood
                       join ft in listFoodType
                       on f.FoodTypeId equals ft.Id
                       select f.ToFoodResponse(ft.FoodTypeName);
            return list;
        }
    }
}