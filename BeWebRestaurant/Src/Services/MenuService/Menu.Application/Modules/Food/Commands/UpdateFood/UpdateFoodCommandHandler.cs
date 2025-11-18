using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using MediatR;
using Menu.Application.DTOs.Responses.Food;
using Menu.Application.IUnitOfWork;
using Menu.Application.Mapping.FoodMapExtension;
using Menu.Domain.Common.Messages.FieldNames;
using Menu.Domain.ValueObjects.Food;
using Microsoft.Extensions.Logging;

namespace Menu.Application.Modules.Food.Commands.UpdateFood
{
    public sealed class UpdateFoodCommandHandler : IRequestHandler<UpdateFoodCommand, FoodResponse>
    {
        private readonly IMenuUnitOfWork _uow;
        private readonly ILogger<UpdateFoodCommandHandler> _logger;

        public UpdateFoodCommandHandler(IMenuUnitOfWork uow, ILogger<UpdateFoodCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<FoodResponse> Handle(UpdateFoodCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var food = await _uow.RFoodRepo.GetByIdAsync(command.IdFood, token);
                if (food is null)
                {
                    _logger.LogWarning("Update failed: Food with Id={Id} not found", command.IdFood);
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.NotFound,
                        FoodField.IdFood,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                    {ParamField.Value,command.IdFood }
                        });
                }

                var foodType = await _uow.RFoodTypeRepo.GetByIdAsync(command.Request.FoodTypeId, token);
                if (foodType is null)
                {
                    _logger.LogWarning("Update failed: FoodType with Id={Id} not found", command.Request.FoodTypeId);
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.NotFound,
                        FoodTypeField.IdFoodType,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                    {ParamField.Value,command.Request.FoodTypeId }
                        });
                }

                var newName = FoodName.Create(command.Request.FoodName);
                if (await _uow.RFoodRepo.ExistsByNameAsync(newName, token, food.Id))
                {
                    _logger.LogWarning("Update failed: Food with Name '{Name}' already exists", newName.Value);
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.Conflict,
                        FoodField.FoodName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<string, object>
                        {
                    {ParamField.Value,newName.Value }
                        });
                }

                food.ApplyFood(command.Request);
                _uow.WFoodRepo.Update(food);
                await _uow.CommitTransactionAsync(token);

                return food.ToFoodResponse(foodType!.FoodTypeName);
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(
                    bex,
                    "BusinessRule Exception occurred while updating Food with Id={Id}. Request: {@Request}",
                    command.IdFood,
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogError(
                    ex,
                    "Exception occurred while updating Food with Id={Id}. Request: {@Request}",
                    command.IdFood,
                    command.Request
                );
                throw;
            }
        }
    }
}