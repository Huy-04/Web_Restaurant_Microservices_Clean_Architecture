using Menu.Domain.Entities;
using Menu.Domain.ValueObjects.FoodType;

namespace Menu.Domain.IRepository
{
    public interface IFoodTypeRepository
    {
        Task<IEnumerable<FoodType>> GetAllAsync(CancellationToken token = default);

        Task<FoodType?> GetByIdAsync(Guid idFoodType, CancellationToken token = default);

        Task<FoodType> CreateAsync(FoodType foodType, CancellationToken token = default);

        Task<FoodType> Update(FoodType foodType);

        Task<bool> DeleteAsync(Guid idFoodType, CancellationToken token = default);

        Task<bool> ExistsByNameAsync(FoodTypeName foodTypeName, CancellationToken token = default, Guid? idFoodType = null);

        Task<bool> ExistsByIdAsync(Guid idFoodType, CancellationToken token);
    }
}