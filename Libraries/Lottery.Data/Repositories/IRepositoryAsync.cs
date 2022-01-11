using Lottery.Core.Caching;
using Lottery.Core.Models;
using System.Linq.Expressions;

namespace Lottery.Data.Repositories
{
    public interface IRepositoryAsync<TEntity> where TEntity : BaseEntity, IBaseEntity
    {
        #region Get

        Task<TEntity> GetByIdAsync(Guid id, Func<IStaticCacheManager, CacheKey> getCacheKey = null, bool asTracking = false);

        Task<IList<TEntity>> GetByIdsAsync(IList<Guid> ids, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        Task<IList<TEntity>> GetAllAsync(Func<IEnumerable<TEntity>, Task<IEnumerable<TEntity>>> func = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null);

        #endregion

        #region Insert

        Task<TEntity> InsertAsync(TEntity entity, bool publishEvent = true);

        Task InsertAsync(IEnumerable<TEntity> entities, bool publishEvent = true);

        #endregion

        #region Update

        Task<bool> UpdateAsync(TEntity entity, bool publishEvent = true);

        Task<bool> UpdateAsync(IEnumerable<TEntity> entities, bool publishEvent = true);

        #endregion

        #region Delete

        Task<bool> DeleteAsync(TEntity entity, bool removeFromTable = false, bool publishEvent = true);

        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> DeleteAsync(IList<TEntity> entities, bool removeFromTable = false, bool publishEvent = true);

        #endregion
    }
}
