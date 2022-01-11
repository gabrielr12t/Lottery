using Lottery.Core.Events;
using Lottery.Core.Models;

namespace Lottery.Services.Events
{
    public abstract partial class EventConsumer<TEntity> :
        IConsumer<EntityInsertedEvent<TEntity>>,
        IConsumer<EntityUpdatedEvent<TEntity>>,
        IConsumer<EntityDeletedEvent<TEntity>>
        where TEntity : BaseEntity, IBaseEntity
    {

        #region Methods

        public virtual async Task HandleEventAsync(EntityInsertedEvent<TEntity> eventMessage)
        {
            await HandleEventAsync(eventMessage.Entity, EntityEventType.Insert);
        }

        public virtual async Task HandleEventAsync(EntityUpdatedEvent<TEntity> eventMessage)
        {
            await HandleEventAsync(eventMessage.Entity, EntityEventType.Update);
        }

        public virtual async Task HandleEventAsync(EntityDeletedEvent<TEntity> eventMessage)
        {
            await HandleEventAsync(eventMessage.Entity, EntityEventType.Delete);
        }

        public virtual Task HandleEventAsync(TEntity entity, EntityEventType delete)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region Nested

        public enum EntityEventType
        {
            Insert,
            Update,
            Delete
        }

        #endregion
    }
}
