using Lottery.Core.Models;

namespace Lottery.Core.Events
{
    public class EntityDeletedEvent<T> where T : IBaseEntity
    {
        public EntityDeletedEvent(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }
}
