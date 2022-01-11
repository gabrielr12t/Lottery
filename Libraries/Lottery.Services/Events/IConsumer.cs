namespace Lottery.Services.Events
{
    public interface IConsumer<T>
    {
        Task HandleEventAsync(T eventMessage);
    }
}
