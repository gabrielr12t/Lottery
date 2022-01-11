using Lottery.Core.Models;

namespace Lottery.Core.Events
{
    public class EntityUpdatedEvent<T> where T : IBaseEntity
    {
        public EntityUpdatedEvent(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }
}
