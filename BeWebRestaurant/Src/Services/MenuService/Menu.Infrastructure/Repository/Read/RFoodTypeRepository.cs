using Infrastructure.Core.Repository;
using Menu.Domain.Entities;
using Menu.Domain.IRepository.Read;
using Menu.Domain.ValueObjects.FoodType;
using Menu.Infrastructure.Persistence;

namespace Infrastructure.Menu.Repository.Read
{
    public class RFoodTypeRepository : ReadRepositoryGeneric<FoodType>, IRFoodTypeRepository
    {
        public RFoodTypeRepository(MenuDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByNameAsync(FoodTypeName foodTypeName, CancellationToken token, Guid? idFoodtype = null)
        {
            return await AnyAsync(ft => (idFoodtype == null || ft.Id != idFoodtype)
            && ft.FoodTypeName == foodTypeName, token);
        }
    }
}