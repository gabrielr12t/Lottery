using Lottery.Core.Events;
using Lottery.Core.Infrastructure;
using Lottery.Services.Logging;

namespace Lottery.Services.Events
{
    public partial class EventPublisher : IEventPublisher
    {
        public virtual async Task PublishAsync<TEvent>(TEvent @event)
        {
            //get all event consumers
            var consumers = EngineContext.Current.ResolveAll<IConsumer<TEvent>>().ToList();

            foreach (var consumer in consumers)
            {
                try
                {
                    await consumer.HandleEventAsync(@event);
                }
                catch (Exception exception)
                {
                    try
                    {
                        var logger = EngineContext.Current.Resolve<ILogger>();
                        if (logger == null)
                            return;

                        await logger.ErrorAsync(exception.Message, exception);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }
    }
}
