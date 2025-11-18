using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using MediatR;
using Menu.Application.IUnitOfWork;
using Menu.Domain.Common.Messages.FieldNames;
using Microsoft.Extensions.Logging;

namespace Menu.Application.Modules.FoodType.Commands.DeleteFoodType
{
    public sealed class DeleteFoodTypeCommandHandler : IRequestHandler<DeleteFoodTypeCommand, bool>
    {
        private readonly IMenuUnitOfWork _uow;
        private readonly ILogger<DeleteFoodTypeCommandHandler> _logger;

        public DeleteFoodTypeCommandHandler(IMenuUnitOfWork uow, ILogger<DeleteFoodTypeCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteFoodTypeCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var foodType = await _uow.RFoodTypeRepo.GetByIdAsync(command.IdFoodType, token);
                if (foodType is null)
                {
                    _logger.LogWarning("Delete failed: FoodType with Id={Id} not found", command.IdFoodType);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        FoodTypeField.IdFoodType,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.IdFoodType }
                        });
                }

                var inUse = await _uow.RFoodRepo.GetByFoodTypeAsync(command.IdFoodType, token);
                if (inUse.Any())
                {
                    _logger.LogWarning("Delete failed: FoodType with Id={Id} is in use by {Count} food items", command.IdFoodType, inUse.Count());
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.Conflict,
                        FoodTypeField.IdFoodType,
                        ErrorCode.InUse,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.IdFoodType }
                        });
                }

                _uow.WFoodTypeRepo.Remove(foodType);
                await _uow.CommitTransactionAsync(token);

                return true;
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(
                    bex,
                    "BusinessRule Exception occurred while deleting FoodType with Id={Id}",
                    command.IdFoodType
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogError(
                    ex,
                    "Exception occurred while deleting FoodType with Id={Id}",
                    command.IdFoodType
                );
                throw;
            }
        }
    }
}