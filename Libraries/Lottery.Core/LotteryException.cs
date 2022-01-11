using System.Runtime.Serialization;

namespace Lottery.Core
{
    [Serializable]
    public class LotteryException : Exception
    {
        public LotteryException()
        {
        }

        public LotteryException(string message)
            : base(message)
        {
        }

        public LotteryException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        protected LotteryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public LotteryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
