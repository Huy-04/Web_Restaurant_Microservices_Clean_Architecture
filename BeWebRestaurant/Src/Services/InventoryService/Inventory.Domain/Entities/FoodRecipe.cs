using Domain.Core.Base;
using Domain.Core.ValueObjects;
using Inventory.Domain.Events.InventoryEvents;

namespace Inventory.Domain.Entities
{
    public sealed class FoodRecipe : AggregateRoot
    {
        public Guid FoodId { get; private set; }

        public Guid IngredientsId { get; private set; }

        public Measurement Measurement { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        // for orm
        private FoodRecipe()
        {
        }

        private FoodRecipe(Guid id, Guid foodId, Guid ingredientsId, Measurement measurement) : base(id)
        {
            FoodId = foodId;
            IngredientsId = ingredientsId;
            Measurement = measurement;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static FoodRecipe Create(Guid foodId, Guid ingredientsId, Measurement measurement)
        {
            var entity = new FoodRecipe(Guid.NewGuid(), foodId, ingredientsId, measurement);
            entity.AddDomainEvent(new FoodRecipeCreatedEvent(entity.Id, entity.FoodId, entity.IngredientsId));
            return entity;
        }

        public void Update(Guid foodId, Guid ingredientsId, Measurement measurement)
        {
            if (FoodId == foodId && IngredientsId == ingredientsId && Measurement == measurement) return;
            FoodId = foodId;
            IngredientsId = ingredientsId;
            Measurement = measurement;
            Touch();
            AddDomainEvent(new FoodRecipeUpdatedEvent(Id, UpdatedAt));
        }

        public void Delete()
        {
            Touch();
            AddDomainEvent(new FoodRecipeDeletedEvent(Id, UpdatedAt));
        }

        private void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
