using Domain.Core.Base;
using Domain.Core.Interface.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Core.Repository
{
    public class ReadRepositoryGeneric<TEntity> : IReadRepositoryGeneric<TEntity>
        where TEntity : Entity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public ReadRepositoryGeneric(DbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default)
        {
            return await _entities
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return await _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, token);
        }

        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            return await _entities
                .AsNoTracking()
                .AnyAsync(e => e.Id == id, token);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _entities
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(token);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _entities.AsNoTracking().AnyAsync(predicate, token);
        }
    }
}