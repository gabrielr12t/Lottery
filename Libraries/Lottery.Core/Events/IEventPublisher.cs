namespace Lottery.Core.Events
{
    public partial interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event);
    }
}
