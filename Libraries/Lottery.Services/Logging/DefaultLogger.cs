using Lottery.Core.Logging;
using Lottery.Data.Repositories;

namespace Lottery.Services.Logging
{
    public partial class DefaultLogger : ILogger
    {
        #region Fields

        private readonly IRepositoryAsync<Log> _logRepository;

        #endregion

        #region Ctor

        public DefaultLogger(
            IRepositoryAsync<Log> logRepository)
        {
            _logRepository = logRepository;
        }

        #endregion

        #region Utilities

        #endregion

        #region Methods

        public virtual bool IsEnabled(LogLevel level)
        {
            return level switch
            {
                LogLevel.Debug => false,
                _ => true,
            };
        }

        public virtual async Task DeleteLogAsync(Log log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            await _logRepository.DeleteAsync(log, false);
        }

        public virtual async Task DeleteLogsAsync(IList<Log> logs)
        {
            await _logRepository.DeleteAsync(logs, false);
        }

        public virtual async Task ClearLogAsync()
        {
            //await _logRepository.dele();
        }

        public virtual async Task<IList<Log?>?> GetAllLogsAsync(
            DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = "", LogLevel? logLevel = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var allLogs = await _logRepository.Table();

            IEnumerable<Log?>? result = null;

            if (fromUtc.HasValue)
                result = allLogs?.Where(l => fromUtc.Value <= l?.CreatedOnUtc);

            if (toUtc.HasValue)
                result = allLogs?.Where(l => toUtc.Value >= l?.CreatedOnUtc);

            if (logLevel.HasValue)
            {
                var logLevelId = (int)logLevel.Value;
                result = allLogs?.Where(l => logLevelId == l?.LogLevelId);
            }

            if (!string.IsNullOrEmpty(message))
                result = allLogs?.Where(l => l.ShortMessage.Contains(message) || l.FullMessage.Contains(message));

            result = allLogs?.OrderByDescending(l => l?.CreatedOnUtc);

            return result?.ToList();
        }

        public virtual async Task<Log> GetLogByIdAsync(Guid logId)
        {
            return await _logRepository.GetByIdAsync(logId);
        }

        public virtual async Task<IList<Log>> GetLogByIdsAsync(Guid[] logIds)
        {
            return await _logRepository.GetByIdsAsync(logIds);
        }

        public virtual async Task<Log> InsertLogAsync(LogLevel logLevel, string shortMessage, string fullMessage = "")
        {
            var log = new Log
            {
                LogLevel = logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                //IpAddress = _webHelper.GetCurrentIpAddress(),
                //PageUrl = _webHelper.GetThisPageUrl(true),
                //ReferrerUrl = _webHelper.GetUrlReferrer(),
            };

            await _logRepository.InsertAsync(log, false);

            return log;
        }

        public virtual async Task InformationAsync(string message, Exception exception = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Information))
                await InsertLogAsync(LogLevel.Information, message, exception?.ToString() ?? string.Empty);
        }

        public virtual async Task WarningAsync(string message, Exception exception = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Warning))
                await InsertLogAsync(LogLevel.Warning, message, exception?.ToString() ?? string.Empty);
        }

        public virtual async Task ErrorAsync(string? message, Exception? exception = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Error))
                await InsertLogAsync(LogLevel.Error, message, exception?.ToString() ?? string.Empty);
        }

        #endregion
    }
}
