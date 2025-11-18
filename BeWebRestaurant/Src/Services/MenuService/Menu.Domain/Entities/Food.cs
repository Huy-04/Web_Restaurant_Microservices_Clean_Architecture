using Domain.Core.ValueObjects;
using Menu.Domain.Enums;
using Menu.Domain.Events.FoodEvents;
using Menu.Domain.Events.FoodRecipeEvents;
using Menu.Domain.ValueObjects.Food;

namespace Menu.Domain.Entities
{
    public sealed class Food : AggregateRoot
    {
        // vo
        public FoodName FoodName { get; private set; } = default!;

        public Money Money { get; private set; } = default!;

        public Img Img { get; private set; } = default!;

        public Description Description { get; private set; } = default!;

        public FoodStatus FoodStatus { get; private set; } = default!;

        // time
        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        // relationship
        public Guid FoodTypeId { get; private set; }

        // collection
        private readonly List<FoodRecipe> _foodRecipes = new List<FoodRecipe>();

        public IReadOnlyCollection<FoodRecipe> FoodRecipes => _foodRecipes.AsReadOnly();

        // for orm
        private Food()
        { }

        private Food(Guid id, FoodName foodName, Money money, Guid foodTypeId, Img img, Description description, FoodStatus foodStatus)
            : base(id)
        {
            FoodName = foodName;
            Money = money;
            FoodTypeId = foodTypeId;
            Img = img;
            Description = description;
            FoodStatus = foodStatus;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Food Create(FoodName foodName, Money money, Guid foodTypeId, Img img, Description description)
        {
            var food = new Food(Guid.NewGuid(), foodName, money, foodTypeId, img, description, FoodStatus.Create(FoodStatusEnum.Active));

            var foodCreatedEvent = new FoodCreatedEvent(food.Id, foodName, description, img, foodTypeId, food.FoodStatus);

            food.AddDomainEvent(foodCreatedEvent);

            return food;
        }

        // behavior
        public void UpdateBasic(FoodName foodName, Img img, Description description)
        {
            if (FoodName == foodName && Img == img && Description == description) return;
            FoodName = foodName;
            Img = img;
            Description = description;
            Touch();
            AddDomainEvent(new FoodUpdatedEvent(
                Id,
                FoodName.Value,
                Money.Amount,
                Img.Value,
                Description.Value,
                FoodStatus.Value.ToString(),
                FoodTypeId,
                UpdatedAt));
        }

        public void UpdateStatus(FoodStatus foodStatus)
        {
            if (FoodStatus == foodStatus) return;
            FoodStatus = foodStatus;
            Touch();
            AddDomainEvent(new FoodUpdatedEvent(
                Id,
                FoodName.Value,
                Money.Amount,
                Img.Value,
                Description.Value,
                FoodStatus.Value.ToString(),
                FoodTypeId,
                UpdatedAt));
        }

        public void MarkAsActive() => UpdateStatus(FoodStatus.Create(FoodStatusEnum.Active));

        public void MarkAsDiscontinued() => UpdateStatus(FoodStatus.Create(FoodStatusEnum.Discontinued));

        public void UpdateFoodTypeId(Guid foodTypeId)
        {
            if (FoodTypeId == foodTypeId) return;
            FoodTypeId = foodTypeId;
            Touch();
            AddDomainEvent(new FoodUpdatedEvent(
                Id,
                FoodName.Value,
                Money.Amount,
                Img.Value,
                Description.Value,
                FoodStatus.Value.ToString(),
                FoodTypeId,
                UpdatedAt));
        }

        public void UpdateMoney(Money money)
        {
            if (Money == money) return;
            Money = money;
            Touch();
            AddDomainEvent(new FoodUpdatedEvent(
                Id,
                FoodName.Value,
                Money.Amount,
                Img.Value,
                Description.Value,
                FoodStatus.Value.ToString(),
                FoodTypeId,
                UpdatedAt));
        }

        public void Delete()
        {
            Touch();
            AddDomainEvent(new FoodDeletedEvent(Id, UpdatedAt));
        }

        // FoodRecipe Management
        public void AddRecipe(Guid ingredientsId, Measurement measurement)
        {
            var recipe = new FoodRecipe(Guid.NewGuid(), Id, ingredientsId, measurement);
            _foodRecipes.Add(recipe);
            Touch();
            AddDomainEvent(new FoodRecipeCreatedEvent(recipe.Id, Id, ingredientsId));
        }

        public void RemoveRecipe(Guid recipeId)
        {
            var recipe = _foodRecipes.FirstOrDefault(r => r.Id == recipeId);
            if (recipe == null) return;

            _foodRecipes.Remove(recipe);
            Touch();
            AddDomainEvent(new FoodRecipeDeletedEvent(recipeId, UpdatedAt));
        }

        public void UpdateRecipeMeasurement(Guid recipeId, Measurement measurement)
        {
            var recipe = _foodRecipes.FirstOrDefault(r => r.Id == recipeId);
            if (recipe == null) return;

            var oldMeasurement = recipe.Measurement;
            recipe.UpdateMeasurement(measurement);

            if (oldMeasurement != measurement)
            {
                Touch();
                AddDomainEvent(new FoodRecipeUpdatedEvent(recipeId, Id, recipe.IngredientsId, measurement.Quantity, UpdatedAt));
            }
        }

        public void UpdateRecipeIngredient(Guid recipeId, Guid ingredientsId)
        {
            var recipe = _foodRecipes.FirstOrDefault(r => r.Id == recipeId);
            if (recipe == null) return;

            var oldIngredientsId = recipe.IngredientsId;
            recipe.UpdateIngredientsId(ingredientsId);

            if (oldIngredientsId != ingredientsId)
            {
                Touch();
                AddDomainEvent(new FoodRecipeUpdatedEvent(recipeId, Id, ingredientsId, recipe.Measurement.Quantity, UpdatedAt));
            }
        }

        // extenstion
        private void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}