using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using MediatR;
using Menu.Application.DTOs.Responses.FoodType;
using Menu.Application.IUnitOfWork;
using Menu.Application.Mapping.FoodTypeMapExtension;
using Menu.Domain.Common.Messages.FieldNames;
using Microsoft.Extensions.Logging;

namespace Menu.Application.Modules.FoodType.Commands.UpdateFoodType
{
    public sealed class UpdateFoodTypeCommandHandler : IRequestHandler<UpdateFoodTypeCommand, FoodTypeResponse>
    {
        private readonly IMenuUnitOfWork _uow;
        private readonly ILogger<UpdateFoodTypeCommandHandler> _logger;

        public UpdateFoodTypeCommandHandler(IMenuUnitOfWork uow, ILogger<UpdateFoodTypeCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<FoodTypeResponse> Handle(UpdateFoodTypeCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var foodType = await _uow.RFoodTypeRepo.GetByIdAsync(command.IdFoodType, token);
                if (foodType is null)
                {
                    _logger.LogWarning("Update failed: FoodType with Id={Id} not found", command.IdFoodType);
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
                if (await _uow.RFoodTypeRepo.ExistsByNameAsync(newName, token, foodType.Id))
                {
                    _logger.LogWarning("Update failed: FoodType with Name '{Name}' already exists", newName.Value);
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
                _uow.WFoodTypeRepo.Update(foodType);
                await _uow.CommitTransactionAsync(token);

                return foodType.ToFoodTypeResponse();
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
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
                await _uow.RollbackTransactionAsync(token);
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