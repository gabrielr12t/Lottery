using Lottery.Core;
using Lottery.Core.Caching;
using Lottery.Core.Events;
using Lottery.Core.Infrastructure;
using Lottery.Core.Models;
using System.Linq.Expressions;
using System.Text.Json;

namespace Lottery.Data.Repositories
{
    public class RepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : BaseEntity, IBaseEntity
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ILotteryFileProvider _fileProvider;

        #endregion

        #region Ctor

        public RepositoryAsync(IEventPublisher eventPublisher,
            IStaticCacheManager staticCacheManager,
            ILotteryFileProvider provider)
        {
            _eventPublisher = eventPublisher;
            _staticCacheManager = staticCacheManager;
            _fileProvider = provider;

            CreateEntityPathAsync();
        }

        #endregion

        #region Get

        public async Task<IList<TEntity>> GetAllAsync(
            Func<IEnumerable<TEntity>, Task<IEnumerable<TEntity>>> func = null,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            async Task<IList<TEntity>> getAllAsync()
            {
                var query = func != null ? func(await Table()) : Table();
                return (await query).ToList();
            }

            return await GetEntitiesAsync(getAllAsync, getCacheKey);
        }

        public async Task<TEntity> GetByIdAsync(
            Guid id,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null,
            bool asTracking = false)
        {
            if (id == Guid.Empty)
                return default;

            async Task<TEntity> getEntity() => (await Table())?.FirstOrDefault(p => p.Id == id);

            if (getCacheKey == null)
                return await getEntity();

            var cacheKey = getCacheKey(_staticCacheManager)
                ?? _staticCacheManager.PrepareKeyForDefaultCache(LotteryEntityCacheDefaults<TEntity>.ByIdCacheKey, id);

            return await _staticCacheManager.GetAsync(cacheKey, getEntity);
        }

        public async Task<IList<TEntity>> GetByIdsAsync(
            IList<Guid> ids,
            Func<IStaticCacheManager, CacheKey> getCacheKey = null)
        {
            if (!ids?.Any() ?? true)
                return new List<TEntity>();

            async Task<IList<TEntity>> getByIds()
            {
                var query = await Table();
                query = query.Where(p => !p.HasDeleted);

                List<TEntity> entries = query.Where(entry => ids.Contains(entry.Id)).ToList();

                var sortedEntries = new List<TEntity>();
                foreach (var id in ids)
                {
                    var sortedEntry = entries.FirstOrDefault(entry => entry.Id == id);
                    if (sortedEntry != null)
                        sortedEntries.Add(sortedEntry);
                }

                return sortedEntries;
            }

            if (getCacheKey == null)
                return await getByIds();

            var cacheKey = getCacheKey(_staticCacheManager)
                ?? _staticCacheManager.PrepareKeyForDefaultCache(LotteryEntityCacheDefaults<TEntity>.ByIdsCacheKey, ids);
            return await _staticCacheManager.GetAsync(cacheKey, getByIds);
        }

        #endregion

        public async Task<bool> DeleteAsync(TEntity entity, bool removeFromTable = false, bool publishEvent = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.DeleteEntity();

            var entities = await GetAllAsync();

            var search = entities.FirstOrDefault(p => p.Id == entity?.Id);
            if (search == null)
                return false;

            return await UpdateAsync(entities);
        }

        public Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(IList<TEntity> entities, bool removeFromTable = false, bool publishEvent = true)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> InsertAsync(TEntity entity, bool publishEvent = true)
        {
            await InsertAsync(new List<TEntity> { entity }, publishEvent);

            return entity;
        }

        public async Task InsertAsync(IEnumerable<TEntity> entities, bool publishEvent = true)
        {
            var allEntities = await GetAllAsync();

            foreach (var entity in entities)
                allEntities.Add(entity);

            if (publishEvent)
                foreach (var entity in allEntities)
                    await _eventPublisher.PublishAsync(entity);

            await File.WriteAllTextAsync(GetEntityFilePath(), JsonSerializer.Serialize(allEntities));
        }

        public async Task<bool> UpdateAsync(TEntity entity, bool publishEvent = true)
        {
            return await UpdateAsync(new List<TEntity> { entity }, publishEvent);
        }

        public async Task<bool> UpdateAsync(IEnumerable<TEntity> entities, bool publishEvent = true)
        {
            if (entities == null)
                throw new LotteryException($"{nameof(entities)} is null");

            if (!entities.Any())
                return false;

            var allEntities = await GetAllAsync();

            foreach (var entity in entities)
            {
                var obj = allEntities.FirstOrDefault(x => x.Id == entity.Id);
                obj = entity;
            }

            if (publishEvent)
                foreach (var entity in entities)
                    await _eventPublisher.EntityUpdated(entity);

            return true;
        }

        #region Utilities

        protected virtual async Task<IList<TEntity>> GetEntitiesAsync(
            Func<Task<IList<TEntity>>> getAllAsync,
            Func<IStaticCacheManager, CacheKey> getCacheKey)
        {
            if (getCacheKey == null)
                return await getAllAsync();

            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForDefaultCache(LotteryEntityCacheDefaults<TEntity>.AllCacheKey);
            return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
        }

        protected virtual IList<TEntity> GetEntities(Func<IList<TEntity>> getAll, Func<IStaticCacheManager, CacheKey> getCacheKey)
        {
            if (getCacheKey == null)
                return getAll();

            var cacheKey = getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForDefaultCache(LotteryEntityCacheDefaults<TEntity>.AllCacheKey);

            return _staticCacheManager.Get(cacheKey, getAll);
        }

        protected virtual async Task<IList<TEntity>> GetEntitiesAsync(Func<Task<IList<TEntity>>> getAllAsync, Func<IStaticCacheManager, Task<CacheKey>> getCacheKey)
        {
            if (getCacheKey == null)
                return await getAllAsync();

            var cacheKey = await getCacheKey(_staticCacheManager)
                           ?? _staticCacheManager.PrepareKeyForDefaultCache(LotteryEntityCacheDefaults<TEntity>.AllCacheKey);
            return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
        }

        private async Task<IEnumerable<TEntity>> Table()
        {
            async Task<IEnumerable<TEntity>> getAllAsync()
            {
                var entitiesJson = _fileProvider.FileExists(GetEntityFilePath()) ?
                    (await File.ReadAllTextAsync(GetEntityFilePath())) : null;

                return !string.IsNullOrEmpty(entitiesJson) ? 
                    JsonSerializer.Deserialize<IEnumerable<TEntity>>(entitiesJson) : Enumerable.Empty<TEntity>();
            }

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(LotteryEntityCacheDefaults<TEntity>.AllCacheKey);
            return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
        }

        private void CreateEntityPathAsync()
        {
            _fileProvider.CreateFile(GetEntityFilePath());
        }

        private string GetEntityFilePath()
        {
            return _fileProvider.MapPath(LotteryEntitySettingsDefaults<TEntity>.EntityFilePath);
        }

        #endregion
    }
}
