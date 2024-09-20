namespace BookLibrary.Domain.SeedWork
{
    public abstract class Entity : IAggregateRoot
    {
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }

    public abstract class Entity<TId> : Entity
    {
        public TId Id { get; protected set; }

        protected Entity(TId id)
        {
            if (object.Equals(id, default(TId)))
            {
                throw new ArgumentException("The ID cannot be the type's default value.", nameof(id));
            }

            Id = id;
        }

        protected Entity()
        {
        }
    }
}
