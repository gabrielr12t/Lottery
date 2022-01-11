namespace Lottery.Core.Models
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }

        DateTime CreatedOnUtc { get; }

        DateTime? UpdatedOnUtc { get; }

        DateTime? DeletedOnUtc { get; }

        bool HasDeleted { get; }

        void UpdateEntity();

        void DeleteEntity();
    }
}
