using Genesis.CoreApi.Shared;

namespace Genesis.CoreApi.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetAll();
        Task<NProcessResult<TEntity>> GetId(int id);
        Task<NProcessResult<TEntity>> Create(TEntity entity);
        Task<NProcessResult<bool>> Update(TEntity entity);
        Task<NProcessResult<bool>> Delete(int id);
    }
}
