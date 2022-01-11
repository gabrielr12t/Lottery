namespace Lottery.Core.Infrastructure
{
    public class LotteryEnvironment : ILotteryEnvironment
    {
        public LotteryEnvironment(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; private set; }
    }
}
