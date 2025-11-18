using Domain.Core.Interface.IRepository;
using Menu.Domain.Entities;
using Menu.Domain.ValueObjects.FoodType;

namespace Menu.Domain.IRepository.Read
{
    public interface IRFoodTypeRepository : IReadRepositoryGeneric<FoodType>
    {
        Task<bool> ExistsByNameAsync(FoodTypeName foodTypeName, CancellationToken token, Guid? idFoodtype = null);
    }
}