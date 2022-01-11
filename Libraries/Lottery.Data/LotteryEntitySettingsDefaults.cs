using Lottery.Core.Models;

namespace Lottery.Data
{
    public static class LotteryEntitySettingsDefaults<TEntity>
        where TEntity : IBaseEntity
    {
        public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();

        public static string EntityFilePath => $"{LotteryDataSettingsDefaults.DataPath}/{EntityTypeName}.json";
    }
}
