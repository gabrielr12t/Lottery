using Lottery.Core.Caching;
using Lottery.Core.Events;
using Lottery.Core.Infrastructure;
using Lottery.Core.Models;
using Lottery.Services.Events;

namespace Lottery.Services.Caching
{
    public abstract partial class CacheEventConsumer<TEntity> :
        IConsumer<EntityInsertedEvent<TEntity>>,
        IConsumer<EntityUpdatedEvent<TEntity>>,
        IConsumer<EntityDeletedEvent<TEntity>>
        where TEntity : BaseEntity, IBaseEntity
    {
        #region Fields

        protected readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        protected CacheEventConsumer()
        {
            _staticCacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
        }

        #endregion

        #region Utilities

        protected virtual async Task ClearCacheAsync(TEntity entity, EntityEventType entityEventType)
        {
            await RemoveByPrefixAsync(LotteryEntityCacheDefaults<TEntity>.ByIdsPrefix);
            await RemoveByPrefixAsync(LotteryEntityCacheDefaults<TEntity>.AllPrefix);

            if (entityEventType != EntityEventType.Insert)
                await RemoveAsync(LotteryEntityCacheDefaults<TEntity>.ByIdCacheKey, entity);

            await ClearCacheAsync(entity);
        }

        protected virtual Task ClearCacheAsync(TEntity entity)
        {
            return Task.CompletedTask;
        }

        protected virtual async Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
        {
            await _staticCacheManager.RemoveByPrefixAsync(prefix, prefixParameters);
        }

        public async Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
        {
            await _staticCacheManager.RemoveAsync(cacheKey, cacheKeyParameters);
        }

        #endregion

        #region Methods

        public virtual async Task HandleEventAsync(EntityInsertedEvent<TEntity> eventMessage)
        {
            await ClearCacheAsync(eventMessage.Entity, EntityEventType.Insert);
        }

        public virtual async Task HandleEventAsync(EntityUpdatedEvent<TEntity> eventMessage)
        {
            await ClearCacheAsync(eventMessage.Entity, EntityEventType.Update);
        }

        public virtual async Task HandleEventAsync(EntityDeletedEvent<TEntity> eventMessage)
        {
            await ClearCacheAsync(eventMessage.Entity, EntityEventType.Delete);
        }

        #endregion

        #region Nested

        protected enum EntityEventType
        {
            Insert,
            Update,
            Delete
        }

        #endregion
    }
}
