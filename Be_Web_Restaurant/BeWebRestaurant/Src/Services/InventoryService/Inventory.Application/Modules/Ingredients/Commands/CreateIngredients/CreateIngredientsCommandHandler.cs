using Domain.Core.Base;
using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Inventory.Application.DTOs.Responses.Ingredients;
using Inventory.Application.Interfaces;
using Inventory.Application.Mapping.IngredientsMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.Ingredients.Commands.CreateIngredients
{
    public sealed class CreateIngredientsCommandHandler : IRequestHandler<CreateIngredientsCommand, IngredientsResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CreateIngredientsCommandHandler> _logger;

        public CreateIngredientsCommandHandler(IUnitOfWork uow, ILogger<CreateIngredientsCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<IngredientsResponse> Handle(CreateIngredientsCommand command, CancellationToken token)
        {
            _logger.LogInformation("Handling create Ingredients");

            await _uow.BeginTransactionAsync(token);
            try
            {
                var ingredients = command.Request.ToIngredients();
                if (await _uow.IngredientsRepo.ExistsByNameAsync(ingredients.IngredientsName, token))
                {
                    _logger.LogWarning(
                      "Create failed: Ingredients with Name:'{Name}' already exists",
                      ingredients.IngredientsName.Value
                  );
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.Conflict,
                        IngredientsField.IngredientsName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<string, object>
                        {
                                {ParamField.Value,ingredients.IngredientsName.Value }
                        });
                }

                await _uow.IngredientsRepo.CreateAsync(ingredients, token);
                await _uow.CommitAsync(token);
                
                _logger.LogInformation(
                   "Successfully created Ingredients with Id={Id}",
                   ingredients.Id
               );
                return ingredients.ToIngredientsResponse();
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogWarning(bex,
                    "BusinessRule Exception occurred while creating Ingredients. Request: {@Request}",
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while creating Ingredients. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}