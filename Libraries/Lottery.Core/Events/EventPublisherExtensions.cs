using Lottery.Core.Models;

namespace Lottery.Core.Events
{
    public static class EventPublisherExtensions
    {
        public static async Task EntityInsertedAsync<T>(this IEventPublisher eventPublisher, T entity) where T : IBaseEntity
        {
            await eventPublisher.PublishAsync(new EntityInsertedEvent<T>(entity));
        }

        public static async Task EntityUpdated<T>(this IEventPublisher eventPublisher, T entity) where T : IBaseEntity
        {
            await eventPublisher.PublishAsync(new EntityUpdatedEvent<T>(entity));
        }

        public static async Task EntityDeleted<T>(this IEventPublisher eventPublisher, T entity) where T : IBaseEntity
        {
            await eventPublisher.PublishAsync(new EntityDeletedEvent<T>(entity));
        }
    }
}
