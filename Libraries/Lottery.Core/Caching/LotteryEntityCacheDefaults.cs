using Lottery.Core.Models;

namespace Lottery.Core.Caching
{
    public static partial class LotteryEntityCacheDefaults<TEntity>
        where TEntity : IBaseEntity
    {
        public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();

        public static CacheKey ByIdCacheKey => new CacheKey($"Lottery.{EntityTypeName}.byid.{{0}}", ByIdPrefix, Prefix);

        public static CacheKey ByIdsCacheKey => new CacheKey($"Lottery.{EntityTypeName}.byids.{{0}}", ByIdsPrefix, Prefix);

        public static CacheKey AllCacheKey => new CacheKey($"Lottery.{EntityTypeName}.all.", AllPrefix, Prefix);

        public static string Prefix => $"Lottery.{EntityTypeName}.";

        public static string ByIdPrefix => $"Lottery.{EntityTypeName}.byid.";

        public static string ByIdsPrefix => $"Lottery.{EntityTypeName}.byids.";

        public static string AllPrefix => $"Lottery.{EntityTypeName}.all.";
    }
}
