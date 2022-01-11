namespace Lottery.Core.Models
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }

        DateTime CreatedOnUtc { get; set; }

        DateTime? UpdatedOnUtc { get; set; }

        DateTime? DeletedOnUtc { get; set; }

        bool HasDeleted { get; }

        void UpdateEntity();

        void DeleteEntity();
    }
}
