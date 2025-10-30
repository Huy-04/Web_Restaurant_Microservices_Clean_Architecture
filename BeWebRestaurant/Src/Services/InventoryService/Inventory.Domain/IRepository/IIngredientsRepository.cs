using Inventory.Domain.Entities;
using Inventory.Domain.ValueObjects.Ingredients;

namespace Inventory.Domain.IRepository
{
    public interface IIngredientsRepository
    {
        Task<IEnumerable<Ingredients>> GetAllAsync(CancellationToken token);

        Task<Ingredients?> GetByIdAsync(Guid idIngredients, CancellationToken token = default);

        Task<Ingredients> CreateAsync(Ingredients ingredients, CancellationToken token = default);

        Task<Ingredients> Update(Ingredients ingredients);

        Task<bool> DeleteAsync(Guid idIngredients, CancellationToken token = default);

        Task<bool> ExistsByNameAsync(IngredientsName ingredientsName, CancellationToken token = default, Guid? idIngredients = null);

        Task<bool> ExistsByIdAsync(Guid idStock, CancellationToken token = default);
    }
}