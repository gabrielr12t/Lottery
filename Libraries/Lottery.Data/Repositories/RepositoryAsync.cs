using Lottery.Core;
using Lottery.Core.Events;
using Lottery.Core.Infrastructure;
using Lottery.Core.Models;
using System.Linq.Expressions;
using System.Text.Json;
using System.Linq;

namespace Lottery.Data.Repositories
{
    public class RepositoryAsync<TEntity> : IRepositoryAsync<TEntity>
        where TEntity : BaseEntity, IBaseEntity
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly ILotteryFileProvider _fileProvider;

        #endregion

        #region Ctor

        public RepositoryAsync(IEventPublisher eventPublisher,
            ILotteryFileProvider provider)
        {
            _eventPublisher = eventPublisher;
            _fileProvider = provider;

            CreateEntityPathAsync();
        }

        #endregion

        #region Get

        public async Task<TEntity?> GetByIdAsync(
            Guid id)
        {
            if (id == Guid.Empty)
                return default;

            return (await Table())?.FirstOrDefault(p => p?.Id == id);
        }

        public async Task<IList<TEntity>> GetByIdsAsync(
            IList<Guid> ids)
        {
            if (!ids?.Any() ?? true)
                return new List<TEntity>();

            var query = await Table();
            query = query?.Where(p => !p.HasDeleted)?.ToList();

            var entries = query?.Where(entry => ids?.Contains(entry.Id) ?? false);

            return (from id in ids
                    let sortedEntry = entries?.FirstOrDefault(entry => entry?.Id == id)
                    where sortedEntry != null
                    select sortedEntry).ToList();
        }

        #endregion

        public async Task<bool> DeleteAsync(TEntity entity, bool removeFromTable = false, bool publishEvent = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.DeleteEntity();

            var entities = await Table();

            var search = entities?.FirstOrDefault(p => p?.Id == entity?.Id);
            if (search == null)
                return false;

            return await UpdateAsync(entities);
        }

        public Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(IList<TEntity> entities, bool removeFromTable = false, bool publishEvent = true)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> InsertAsync(TEntity entity, bool publishEvent = true)
        {
            await InsertAsync(new List<TEntity> { entity }, publishEvent);

            return entity;
        }

        public async Task InsertAsync(IEnumerable<TEntity> entities, bool publishEvent = true)
        {
            var allEntities = await Table();

            foreach (var entity in entities)
                allEntities?.Add(entity);

            if (publishEvent)
                foreach (var entity in allEntities)
                    await _eventPublisher.PublishAsync(entity);

            await File.WriteAllTextAsync(GetEntityFilePath(), JsonSerializer.Serialize(allEntities));
        }

        public async Task<bool> UpdateAsync(TEntity entity, bool publishEvent = true)
        {
            return await UpdateAsync(new List<TEntity> { entity }, publishEvent);
        }

        public async Task<bool> UpdateAsync(IEnumerable<TEntity?>? entities, bool publishEvent = true)
        {
            if (entities == null)
                throw new LotteryException($"{nameof(entities)} is null");

            if (!entities.Any())
                return false;

            var allEntities = await Table();

            foreach (var entity in entities)
            {
                var obj = allEntities?.FirstOrDefault(x => x?.Id == entity?.Id);
                obj = entity;
            }

            if (publishEvent)
                foreach (var entity in entities)
                    await _eventPublisher.EntityUpdated(entity);

            return true;
        }

        public async Task<IList<TEntity?>?> Table()
        {
            var entitiesJson = _fileProvider.FileExists(GetEntityFilePath()) ?
                (await File.ReadAllTextAsync(GetEntityFilePath()).ConfigureAwait(false)) : null;

            if (string.IsNullOrEmpty(entitiesJson))
                return new List<TEntity?>();

            return JsonSerializer.Deserialize<IList<TEntity?>?>(entitiesJson);
        }

        #region Utilities

        private void CreateEntityPathAsync()
        {
            _fileProvider.CreateFile(GetEntityFilePath());
        }

        private string GetEntityFilePath()
        {
            return _fileProvider.MapPath(LotteryEntitySettingsDefaults<TEntity>.EntityFilePath);
        }

        #endregion
    }
}
