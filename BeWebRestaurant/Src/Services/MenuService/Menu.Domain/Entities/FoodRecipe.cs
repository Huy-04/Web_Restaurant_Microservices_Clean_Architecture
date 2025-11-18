using Domain.Core.Base;
using Domain.Core.ValueObjects;

namespace Menu.Domain.Entities
{
    public sealed class FoodRecipe : Entity
    {
        public Guid FoodId { get; private set; }

        public Guid IngredientsId { get; private set; }

        public Measurement Measurement { get; private set; }

        // for orm
        private FoodRecipe()
        {
        }

        internal FoodRecipe(Guid id, Guid foodId, Guid ingredientsId, Measurement measurement) : base(id)
        {
            FoodId = foodId;
            IngredientsId = ingredientsId;
            Measurement = measurement;
        }

        internal void UpdateMeasurement(Measurement measurement)
        {
            if (Measurement == measurement) return;
            Measurement = measurement;
        }

        internal void UpdateIngredientsId(Guid ingredientsId)
        {
            if (IngredientsId == ingredientsId) return;
            IngredientsId = ingredientsId;
        }
    }
}