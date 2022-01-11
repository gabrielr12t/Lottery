using Lottery.Core.Models;

namespace Lottery.Core.Events
{
    public class EntityInsertedEvent<T> where T : IBaseEntity
    {
        public EntityInsertedEvent(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }
}
