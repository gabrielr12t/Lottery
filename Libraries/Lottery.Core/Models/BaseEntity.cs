namespace Lottery.Core.Models
{
    public class BaseEntity : IBaseEntity, IEquatable<IBaseEntity>, IComparable<IBaseEntity>
    {
        #region Ctor

        public BaseEntity()
        {
            if (CreatedOnUtc == DateTime.MinValue)
                CreatedOnUtc = DateTime.UtcNow;

            if (Id == Guid.Empty)
                Id = Guid.NewGuid();
        }

        #endregion

        #region Properties

        public Guid Id { get; set; }

        public DateTime CreatedOnUtc { get; protected set; }

        public DateTime? DeletedOnUtc { get; protected set; }

        public DateTime? UpdatedOnUtc { get; protected set; }

        public bool HasDeleted { get { return DeletedOnUtc.HasValue; } }

        #endregion

        #region Methods

        public void UpdateEntity()
        {
            UpdatedOnUtc = DateTime.UtcNow;
        }

        public void DeleteEntity()
        {
            if (DeletedOnUtc == null || DeletedOnUtc == DateTime.MinValue)
                DeletedOnUtc = DateTime.UtcNow;
        }

        #endregion

        #region GetHashCode, Equals, IEquatable, IComparable

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = (hash * 7) + Id.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IBaseEntity);
        }

        public virtual int CompareTo(IBaseEntity other)
        {
            return this.Id.CompareTo(other?.Id);
        }

        public virtual bool Equals(IBaseEntity other)
        {
            if (other is null)
                return false;

            return other.Id.Equals(this.Id);
        }

        #endregion
    }
}
