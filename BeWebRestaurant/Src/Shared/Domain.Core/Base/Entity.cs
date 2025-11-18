namespace Domain.Core.Base
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        protected Entity(Guid id) : this()
        {
            if (id != Guid.Empty)
                Id = id;
        }
    }
}