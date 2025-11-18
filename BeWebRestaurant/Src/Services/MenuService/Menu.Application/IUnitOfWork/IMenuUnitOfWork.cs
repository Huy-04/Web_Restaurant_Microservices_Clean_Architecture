using Application.Core.IUnitOfWork;
using Menu.Domain.IRepository.Read;
using Menu.Domain.IRepository.Write;

namespace Menu.Application.IUnitOfWork
{
    public interface IMenuUnitOfWork : IUintOfWorkGeneric
    {
        // Write
        IWFoodRepository WFoodRepo { get; }

        IWFoodTypeRepository WFoodTypeRepo { get; }

        // Read
        IRFoodRepository RFoodRepo { get; }

        IRFoodTypeRepository RFoodTypeRepo { get; }
    }
}