using Domain.Core.Base;
using System.Linq.Expressions;

namespace Domain.Core.Interface.IRepository
{
    public interface IReadRepositoryGeneric<TEntity> where TEntity : Entity
    {
        public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);

        public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token = default);

        public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);

        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

        public Task<IEnumerable<TChild>> FindChildEntityAsync<TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> navigationExpr, Expression<Func<TChild, bool>> childPredicate, CancellationToken token = default);

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);
    }
}