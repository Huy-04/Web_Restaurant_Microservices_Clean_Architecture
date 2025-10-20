using Domain.Core.Base;
using Domain.Core.ValueObjects;
using Inventory.Domain.ValueObjects.Ingredients;

namespace Inventory.Domain.Entities
{
    public sealed class Ingredients : AggregateRoot
    {
        public IngredientsName IngredientsName { get; private set; }

        public Description Description { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        // for orm
        private Ingredients()
        {
        }

        private Ingredients(Guid id, IngredientsName ingredientsName, Description description) : base(id)
        {
            IngredientsName = ingredientsName;
            Description = description;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Ingredients Create(IngredientsName ingredientsName, Description description)
        {
            var entity = new Ingredients(Guid.NewGuid(), ingredientsName, description);

            return entity;
        }

        public void Update(IngredientsName ingredientsName, Description description)
        {
            if (IngredientsName == ingredientsName && Description == description) return;
            IngredientsName = ingredientsName;
            Description = description;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}