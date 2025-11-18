using Infrastructure.Core.IRepository;
using Menu.Domain.Entities;
using Menu.Domain.IRepository.Write;
using Menu.Infrastructure.Persistence;

namespace Menu.Infrastructure.Repository.Write
{
    public class WFoodTypeRepository : WriteRepositoryGeneric<FoodType>, IWFoodTypeRepository
    {
        public WFoodTypeRepository(MenuDbContext context) : base(context)
        {
        }
    }
}