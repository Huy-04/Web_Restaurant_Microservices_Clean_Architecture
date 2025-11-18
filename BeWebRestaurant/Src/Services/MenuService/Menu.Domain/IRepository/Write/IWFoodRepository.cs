using Domain.Core.Interface.IRepository;
using Menu.Domain.Entities;

namespace Menu.Domain.IRepository.Write
{
    public interface IWFoodRepository : IWriteRepositoryGeneric<Food>
    {
        Task<Food?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}