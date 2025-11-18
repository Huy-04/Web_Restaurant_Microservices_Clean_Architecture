using Domain.Core.Interface.IRepository;
using Infrastructure.Core.Repository;
using Menu.Domain.Entities;
using Menu.Domain.IRepository.Write;
using Menu.Infrastructure.Persistence;

namespace Menu.Infrastructure.Repository.Write
{
    // NOTE: This repository is no longer needed as FoodRecipe is now managed within Food aggregate
    // All FoodRecipe operations should go through IWFoodRepository
    
    /*
    public class WFoodRecipeRepository : WriteRepositoryGeneric<FoodRecipe>, IWFoodRecipeRepository
    {
        public WFoodRecipeRepository(MenuDbContext context) : base(context)
        {
        }
    }
    */
}
