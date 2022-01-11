using Lottery.Core.Models;

namespace Lottery.Core.Logging
{
    public partial class Log : BaseEntity, IBaseEntity
    {
        public int LogLevelId { get; set; }

        public string ShortMessage { get; set; }

        public string FullMessage { get; set; }

        public LogLevel LogLevel
        {
            get => (LogLevel)LogLevelId;
            set => LogLevelId = (int)value;
        }
    }
}
