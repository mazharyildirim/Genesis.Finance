using Genesis.CoreApi.Shared;
using Genesis.Shared.Models.UserManagement;

namespace Genesis.CoreApi.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAll(CancellationToken cancellationToken);
        Task<NProcessResult<TEntity>> GetId(int id, CancellationToken cancellationToken);
        Task<NProcessResult<TEntity>> Create(TEntity entity, CancellationToken cancellationToken);
        Task<NProcessResult<bool>> Update(TEntity entity, CancellationToken cancellationToken);
        Task<NProcessResult<bool>> Delete(int id, CancellationToken cancellationToken);
    }
}
