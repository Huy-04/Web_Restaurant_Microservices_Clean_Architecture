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

namespace Menu.Application.Modules.FoodType.Commands.CreateFoodType
{
    public sealed class CreateFoodTypeCommandHandler : IRequestHandler<CreateFoodTypeCommand, FoodTypeResponse>
    {
        private readonly IMenuUnitOfWork _uow;
        private readonly ILogger<CreateFoodTypeCommandHandler> _logger;

        public CreateFoodTypeCommandHandler(IMenuUnitOfWork uow, ILogger<CreateFoodTypeCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<FoodTypeResponse> Handle(CreateFoodTypeCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var newName = command.Request.ToFoodTypeName();
                if (await _uow.RFoodTypeRepo.ExistsByNameAsync(newName, token))
                {
                    _logger.LogWarning("Create failed: FoodType with Name '{Name}' already exists", newName.Value);
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.Conflict,
                        FoodTypeField.FoodTypeName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<string, object>
                        {
                    {ParamField.Value,newName.Value }
                        });
                }

                var foodType = command.Request.ToFoodType();
                await _uow.WFoodTypeRepo.AddAsync(foodType, token);
                await _uow.CommitTransactionAsync(token);

                return foodType.ToFoodTypeResponse();
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(bex,
                    "BusinessRule Exception occurred while creating FoodType. Request: {@Request}",
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while creating FoodType. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}