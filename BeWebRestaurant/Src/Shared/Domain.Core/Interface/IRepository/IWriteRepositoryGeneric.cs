using Domain.Core.Base;

namespace Domain.Core.Interface.IRepository
{
    public interface IWriteRepositoryGeneric<TEntity> where TEntity : Entity
    {
        Task AddAsync(TEntity entity, CancellationToken token = default);

        void Update(TEntity entity);

        void Remove(TEntity entity);
    }
}