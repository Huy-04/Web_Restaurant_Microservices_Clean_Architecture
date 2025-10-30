using Menu.Domain.Entities;
using Menu.Domain.ValueObjects.Food;

namespace Menu.Domain.IRepository
{
    public interface IFoodRepository
    {
        Task<IEnumerable<Food>> GetAllAsync(CancellationToken token = default);

        Task<Food?> GetByIdAsync(Guid idFood, CancellationToken token = default);

        Task<IEnumerable<Food>> GetByFoodTypeAsync(Guid foodTypeId, CancellationToken token = default);

        Task<IEnumerable<Food>> GetByStatusAsync(FoodStatus foodStatus, CancellationToken token = default);

        Task<Food> CreateAsync(Food food, CancellationToken token = default);

        Task<Food> Update(Food food);

        Task<bool> DeleteAsync(Guid idFood, CancellationToken token = default);

        Task<bool> ExistsByNameAsync(FoodName foodName, CancellationToken token = default, Guid? idFood = null);

        Task<bool> ExistsByIdAsync(Guid idFood, CancellationToken token = default);
    }
}