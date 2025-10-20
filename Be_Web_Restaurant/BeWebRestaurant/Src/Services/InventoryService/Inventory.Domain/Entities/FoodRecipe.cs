using Domain.Core.Base;
using Domain.Core.ValueObjects;

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
            return entity;
        }

        public void Update(Guid foodId, Guid ingredientsId, Measurement measurement)
        {
            if (FoodId == foodId && IngredientsId == ingredientsId && Measurement == measurement) return;
            FoodId = foodId;
            IngredientsId = ingredientsId;
            Measurement = measurement;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}