namespace Lottery.Core.Infrastructure
{
    public class LotteryApplicationBuilder : ILotteryApplicationBuilder
    {
        public LotteryApplicationBuilder(IServiceProvider applicationServices)
        {
            ApplicationServices = applicationServices;
        }

        public IServiceProvider ApplicationServices { get; private set; }
    }
}
