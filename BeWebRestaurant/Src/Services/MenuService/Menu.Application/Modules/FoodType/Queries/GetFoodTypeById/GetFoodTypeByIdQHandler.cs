﻿using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using MediatR;
using Menu.Application.DTOs.Responses.FoodType;
using Menu.Application.Interface;
using Menu.Application.Mapping.FoodTypeMapExtension;
using Menu.Domain.Common.Messages.FieldNames;

namespace Menu.Application.Modules.FoodType.Queries.GetFoodTypeById
{
    public sealed class GetFoodTypeByIdQHandler : IRequestHandler<GetFoodTypeByIdQuery, FoodTypeResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetFoodTypeByIdQHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<FoodTypeResponse> Handle(GetFoodTypeByIdQuery query, CancellationToken token)
        {
            var foodType = await _uow.FoodTypeRepo.GetByIdAsync(query.IdFoodType);
            if (foodType is null)
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    FoodTypeField.IdFoodType,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                        {ParamField.Value,query.IdFoodType }
                    });
            }
            return foodType.ToFoodTypeResponse();
        }
    }
}