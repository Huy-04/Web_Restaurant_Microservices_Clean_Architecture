using Infrastructure.Core.UnitOfWork;
using Menu.Application.IUnitOfWork;
using Menu.Domain.IRepository.Read;
using Menu.Domain.IRepository.Write;
using Menu.Infrastructure.Persistence;

namespace Menu.Infrastructure.UnitOfWork
{
    public class MenuUnitOfWork : UnitOfWorkGeneric, IMenuUnitOfWork
    {
        public MenuUnitOfWork(
            MenuDbContext context,
            IRFoodRepository rFoodRepo,
            IWFoodRepository wFoodRepo,
            IRFoodTypeRepository rFoodTypeRepo,
            IWFoodTypeRepository wFoodTypeRepo) : base(context)
        {
            RFoodRepo = rFoodRepo;
            WFoodRepo = wFoodRepo;
            RFoodTypeRepo = rFoodTypeRepo;
            WFoodTypeRepo = wFoodTypeRepo;
        }

        public IWFoodRepository WFoodRepo { get; }

        public IWFoodTypeRepository WFoodTypeRepo { get; }

        public IRFoodRepository RFoodRepo { get; }

        public IRFoodTypeRepository RFoodTypeRepo { get; }
    }
}