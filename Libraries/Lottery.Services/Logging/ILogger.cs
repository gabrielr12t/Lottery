using Lottery.Core.Logging;

namespace Lottery.Services.Logging
{
    public partial interface ILogger
    {
        bool IsEnabled(LogLevel level);

        Task DeleteLogAsync(Log log);

        Task DeleteLogsAsync(IList<Log> logs);

        Task ClearLogAsync();

        Task<IList<Log?>?> GetAllLogsAsync(
             DateTime? fromUtc = null, DateTime? toUtc = null,
             string message = "", LogLevel? logLevel = null,
             int pageIndex = 0, int pageSize = int.MaxValue);

        Task<Log> GetLogByIdAsync(Guid logId);

        Task<IList<Log>> GetLogByIdsAsync(Guid[] logIds);

        Task<Log> InsertLogAsync(LogLevel logLevel, string shortMessage, string fullMessage = "");

        Task InformationAsync(string message, Exception exception = null);

        Task WarningAsync(string message, Exception exception = null);

        Task ErrorAsync(string? message, Exception? exception = null);
    }
}
