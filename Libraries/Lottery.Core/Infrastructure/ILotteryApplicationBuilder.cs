namespace Lottery.Core.Infrastructure
{
    public interface ILotteryApplicationBuilder
    {
        IServiceProvider ApplicationServices { get; }
    }
}
