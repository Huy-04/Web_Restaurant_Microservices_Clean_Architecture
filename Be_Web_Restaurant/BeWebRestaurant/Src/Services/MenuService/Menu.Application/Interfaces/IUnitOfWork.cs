namespace Menu.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IFoodRepository FoodRepo { get; }
        IFoodTypeRepository FoodTypeRepo { get; }

        Task BeginTransactionAsync(CancellationToken token = default);

        Task CommitAsync(CancellationToken token = default);

        Task RollBackAsync(CancellationToken token = default);
    }
}