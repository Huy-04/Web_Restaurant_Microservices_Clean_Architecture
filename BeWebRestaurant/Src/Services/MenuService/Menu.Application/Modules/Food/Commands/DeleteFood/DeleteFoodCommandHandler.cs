using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using MediatR;
using Menu.Application.IUnitOfWork;
using Menu.Domain.Common.Messages.FieldNames;
using Microsoft.Extensions.Logging;

namespace Menu.Application.Modules.Food.Commands.DeleteFood
{
    public sealed class DeleteFoodCommandHandler : IRequestHandler<DeleteFoodCommand, bool>
    {
        private readonly IMenuUnitOfWork _uow;
        private readonly ILogger<DeleteFoodCommandHandler> _logger;

        public DeleteFoodCommandHandler(IMenuUnitOfWork uow, ILogger<DeleteFoodCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteFoodCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var food = await _uow.RFoodRepo.GetByIdAsync(command.IdFood, token);
                if (food is null)
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

                _uow.WFoodRepo.Remove(food);
                await _uow.CommitTransactionAsync(token);

                return true;
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(
                    bex,
                    "BusinessRule Exception occurred while deleting Food with Id={Id}",
                    command.IdFood
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
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