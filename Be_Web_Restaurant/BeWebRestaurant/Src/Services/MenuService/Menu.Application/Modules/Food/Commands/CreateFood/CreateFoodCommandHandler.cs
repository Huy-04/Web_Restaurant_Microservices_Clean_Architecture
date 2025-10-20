using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using MediatR;
using Menu.Application.DTOs.Responses.Food;
using Menu.Application.Interfaces;
using Menu.Application.Mapping.FoodMapExtension;
using Menu.Domain.Common.Messages.FieldNames;
using Microsoft.Extensions.Logging;

namespace Menu.Application.Modules.Food.Commands.CreateFood
{
    public sealed class CreateFoodCommandHandler : IRequestHandler<CreateFoodCommand, FoodResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CreateFoodCommandHandler> _logger;

        public CreateFoodCommandHandler(IUnitOfWork uow, ILogger<CreateFoodCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<FoodResponse> Handle(CreateFoodCommand command, CancellationToken token)
        {
            _logger.LogInformation("Handling create Food");

            await _uow.BeginTransactionAsync(token);
            try
            {
                var foodType = await _uow.FoodTypeRepo.GetByIdAsync(command.Request.FoodTypeId, token);
                if (foodType is null)
                {
                    _logger.LogWarning(
                        "Create failed: FoodType with Id={Id} not found",
                        command.Request.FoodTypeId
                    );
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
                if (await _uow.FoodRepo.ExistsByNameAsync(food.FoodName, token))
                {
                    _logger.LogWarning(
                       "Create failed: Food with Name:'{Name}' already exists",
                       food.FoodName.Value
                   );
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.Conflict,
                        FoodField.FoodName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<string, object>
                        {
                                {ParamField.Value,food.FoodName.Value }
                        });
                }

                await _uow.FoodRepo.CreateAsync(food, token);
                await _uow.CommitAsync(token);
                
                _logger.LogInformation(
                    "Successfully created Food with Id={Id}",
                    food.Id
                );
                return food.ToFoodResponse(foodType.FoodTypeName);
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogWarning(bex,
                    "BusinessRule Exception occurred while creating Food. Request: {@Request}",
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while creating Food. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}