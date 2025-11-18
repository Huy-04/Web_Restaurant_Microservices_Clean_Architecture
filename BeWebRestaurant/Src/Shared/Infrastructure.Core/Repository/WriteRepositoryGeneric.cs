using Domain.Core.Base;
using Domain.Core.Interface.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.IRepository
{
    public class WriteRepositoryGeneric<TEntity> : IWriteRepositoryGeneric<TEntity>
        where TEntity : Entity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public WriteRepositoryGeneric(DbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity, CancellationToken token)
        {
            await _entities.AddAsync(entity, token);
        }

        public void Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }
    }
}