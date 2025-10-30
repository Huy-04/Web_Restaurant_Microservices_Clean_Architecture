using Menu.Domain.IRepository;

namespace Menu.Application.Interface
{
    public interface IUnitOfWork
    {
        IFoodRepository FoodRepo { get; }
        IFoodTypeRepository FoodTypeRepo { get; }

        Task BeginTransactionAsync(CancellationToken token = default);

        Task CommitAsync(CancellationToken token = default);

        Task<int> SaveChangesAsync(CancellationToken token = default);

        Task RollbackAsync(CancellationToken token = default);
    }
}