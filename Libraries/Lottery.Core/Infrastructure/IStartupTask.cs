namespace Lottery.Core.Infrastructure
{
    public interface IStartupTask
    {
        Task ExecuteAsync();

        int Order { get; }
    }
}
