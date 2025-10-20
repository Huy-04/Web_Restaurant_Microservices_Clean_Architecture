using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using MediatR;
using Menu.Application.DTOs.Responses.FoodType;
using Menu.Application.Interfaces;
using Menu.Application.Mapping.FoodTypeMapExtension;
using Menu.Domain.Common.Messages.FieldNames;
using Microsoft.Extensions.Logging;

namespace Menu.Application.Modules.FoodTypes.Commands.UpdateFoodType
{
    public sealed class UpdateFoodTypeCommandHandler : IRequestHandler<UpdateFoodTypeCommand, FoodTypeResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<UpdateFoodTypeCommandHandler> _logger;

        public UpdateFoodTypeCommandHandler(IUnitOfWork uow, ILogger<UpdateFoodTypeCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<FoodTypeResponse> Handle(UpdateFoodTypeCommand command, CancellationToken token)
        {
            _logger.LogInformation(
                "Handling update FoodType with Id={Id}",
                command.IdFoodType
            );

            await _uow.BeginTransactionAsync(token);
            try
            {
                var repo = _uow.FoodTypeRepo;

                var foodType = await repo.GetByIdAsync(command.IdFoodType, token);
                if (foodType is null)
                {
                    _logger.LogWarning(
                        "Update failed: FoodType with Id={Id} not found",
                        command.IdFoodType
                    );
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.NotFound,
                        FoodTypeField.IdFoodType,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                    { ParamField.Value, command.IdFoodType }
                        });
                }

                var newName = command.Request.ToFoodTypeName();
                if (await repo.ExistsByNameAsync(newName, token, foodType.Id))
                {
                    _logger.LogWarning(
                        "Update failed: FoodType with Name:'{Name}' already exists",
                        newName.Value
                    );
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.Conflict,
                        FoodTypeField.FoodTypeName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<string, object>
                        {
                    { ParamField.Value, newName.Value }
                        });
                }

                foodType.UpdateName(newName);
                await repo.Update(foodType);
                await _uow.CommitAsync(token);

                _logger.LogInformation(
                    "Successfully updated FoodType with Id={Id}",
                    command.IdFoodType
                );
                return foodType.ToFoodTypeResponse();
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogWarning(
                    bex,
                    "BusinessRule Exception occurred while updating FoodType with Id={Id}. Request: {@Request}",
                    command.IdFoodType,
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogError(
                    ex,
                    "Exception occurred while updating FoodType with Id={Id}. Request: {@Request}",
                    command.IdFoodType,
                    command.Request
                );
                throw;
            }
        }
    }
}