using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Inventory.Application.DTOs.Responses.Ingredients;
using Inventory.Application.IUnitOfWork;
using Inventory.Application.Mapping.IngredientsMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.Ingredients.Commands.UpdateIngredients
{
    public sealed class UpdateIngredientsCommandHandler : IRequestHandler<UpdateIngredientsCommand, IngredientsResponse>
    {
        private readonly IInventoryUnitOfWork _uow;
        private readonly ILogger<UpdateIngredientsCommandHandler> _logger;

        public UpdateIngredientsCommandHandler(IInventoryUnitOfWork uow, ILogger<UpdateIngredientsCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<IngredientsResponse> Handle(UpdateIngredientsCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var repo = _uow.IngredientsRepo;

                var ingredients = await repo.GetByIdAsync(command.IdIngredients, token);
                if (ingredients is null)
                {
                    _logger.LogWarning("Update failed: Ingredients with Id={Id} not found", command.IdIngredients);
                    throw RuleFactory.SimpleRuleException
                         (ErrorCategory.NotFound,
                         IngredientsField.IdIngredients,
                         ErrorCode.IdNotFound,
                         new Dictionary<string, object>
                         {
                                {ParamField.Value,command.IdIngredients }
                         });
                }

                var entity = command.Request.ToIngredients();
                if (await repo.ExistsByNameAsync(entity.IngredientsName, token, ingredients.Id))
                {
                    _logger.LogWarning("Update failed: Ingredients with Name '{Name}' already exists", entity.IngredientsName.Value);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.Conflict,
                        IngredientsField.IngredientsName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<string, object>
                        {
                                {ParamField.Value,entity.IngredientsName.Value }
                        });
                }

                ingredients.Update(entity.IngredientsName, entity.Description);
                await repo.Update(ingredients);
                await _uow.CommitTransactionAsync(token);

                return ingredients.ToIngredientsResponse();
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(
                    bex,
                    "BusinessRule Exception occurred while updating Ingredients with Id={Id}. Request: {@Request}",
                    command.IdIngredients,
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogError(
                    ex,
                    "Exception occurred while updating Ingredients with Id={Id}. Request: {@Request}",
                    command.IdIngredients,
                    command.Request
                );
                throw;
            }
        }
    }
}

