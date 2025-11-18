using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using MediatR;
using Menu.Application.DTOs.Responses.Food;
using Menu.Application.IUnitOfWork;
using Menu.Application.Mapping.FoodMapExtension;
using Menu.Domain.Common.Messages.FieldNames;
using Microsoft.Extensions.Logging;

namespace Menu.Application.Modules.Food.Commands.CreateFood
{
    public sealed class CreateFoodCommandHandler : IRequestHandler<CreateFoodCommand, FoodResponse>
    {
        private readonly IMenuUnitOfWork _uow;
        private readonly ILogger<CreateFoodCommandHandler> _logger;

        public CreateFoodCommandHandler(IMenuUnitOfWork uow, ILogger<CreateFoodCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<FoodResponse> Handle(CreateFoodCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var foodType = await _uow.RFoodTypeRepo.GetByIdAsync(command.Request.FoodTypeId, token);
                if (foodType is null)
                {
                    _logger.LogWarning("Create failed: FoodType with Id={Id} not found", command.Request.FoodTypeId);
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.NotFound,
                        FoodTypeField.IdFoodType,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                                {ParamField.Value,command.Request.FoodTypeId }
                        });
                }

                var food = command.Request.ToFood();
                if (await _uow.RFoodRepo.ExistsByNameAsync(food.FoodName, token))
                {
                    _logger.LogWarning("Create failed: Food with Name '{Name}' already exists", food.FoodName.Value);
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.Conflict,
                        FoodField.FoodName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<string, object>
                        {
                                {ParamField.Value,food.FoodName.Value }
                        });
                }

                await _uow.WFoodRepo.AddAsync(food, token);
                await _uow.CommitTransactionAsync(token);

                return food.ToFoodResponse(foodType.FoodTypeName);
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(bex,
                    "BusinessRule Exception occurred while creating Food. Request: {@Request}",
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while creating Food. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}