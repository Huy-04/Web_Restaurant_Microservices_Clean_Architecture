using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using MediatR;
using Menu.Application.Interface;
using Menu.Domain.Common.Messages.FieldNames;
using Microsoft.Extensions.Logging;

namespace Menu.Application.Modules.Food.Commands.DeleteFood
{
    public sealed class DeleteFoodCommandHandler : IRequestHandler<DeleteFoodCommand, bool>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<DeleteFoodCommandHandler> _logger;

        public DeleteFoodCommandHandler(IUnitOfWork uow, ILogger<DeleteFoodCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteFoodCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var exists = await _uow.FoodRepo.ExistsByIdAsync(command.IdFood, token);
                if (!exists)
                {
                    _logger.LogWarning("Delete failed: Food with Id={Id} not found", command.IdFood);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        FoodField.IdFood,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.IdFood }
                        });
                }

                await _uow.FoodRepo.DeleteAsync(command.IdFood, token);
                await _uow.SaveChangesAsync(token);
                await _uow.CommitAsync(token);

                return true;
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackAsync(token);
                _logger.LogWarning(
                    bex,
                    "BusinessRule Exception occurred while deleting Food with Id={Id}",
                    command.IdFood
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync(token);
                _logger.LogError(
                    ex,
                    "Exception occurred while deleting Food with Id={Id}",
                    command.IdFood
                );
                throw;
            }
        }
    }
}