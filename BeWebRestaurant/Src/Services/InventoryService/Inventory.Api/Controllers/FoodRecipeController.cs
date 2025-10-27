using Inventory.Application.DTOs.Requests.FoodRecipe;
using Inventory.Application.DTOs.Responses.FoodRecipe;
using Inventory.Application.Modules.FoodRecipe.Commands.CreateFoodRecipe;
using Inventory.Application.Modules.FoodRecipe.Commands.DeleteFoodRecipe;
using Inventory.Application.Modules.FoodRecipe.Commands.UpdateFoodRecipe;
using Inventory.Application.Modules.FoodRecipe.Queries.GetAllFoodRecipe;
using Inventory.Application.Modules.FoodRecipe.Queries.GetByIngredients;
using Inventory.Application.Modules.FoodRecipe.Queries.GetFoodRecipeByFood;
using Inventory.Application.Modules.FoodRecipe.Queries.GetFoodRecipeByFoodAndIngredients;
using Inventory.Application.Modules.FoodRecipe.Queries.GetFoodRecipeById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class FoodRecipeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FoodRecipeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/FoodRecipe
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodRecipeResponse>>> GetAll
            (CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllFoodRecipeQuery(), token);
            return Ok(result);
        }

        // Get: api/FoodRecipe/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FoodRecipeResponse>> GetById
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetFoodRecipeByIdQuery(id), token);
            return Ok(result);
        }

        // Get: api/Fooecipe/food/{id}
        [HttpGet("food/{id:guid}")]
        public async Task<ActionResult<IEnumerable<FoodRecipeResponse>>> GetByFood
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetFoodRecipeByFoodQuery(id), token);
            return Ok(result);
        }

        // Get: api/FoodRecipe/ingredients/{id}
        [HttpGet("ingredients/{id:guid}")]
        public async Task<ActionResult<IEnumerable<FoodRecipeResponse>>> GetByIngredients
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetFoodRecipeByIngredientsQuery(id), token);
            return Ok(result);
        }

        // Get: api/FoodRecipe/food/{id}/ingredients/{id}
        [HttpGet("food/{foodId:guid}/ingredients/{ingredientsId:guid}")]
        public async Task<ActionResult<IEnumerable<FoodRecipeResponse>>> GetByIngredientsAndFood
            ([FromRoute] Guid foodId, [FromRoute] Guid ingredientsId, CancellationToken token)
        {
            var result = await _mediator.Send(new GetFoodRecipeByFoodAndIngredientsQuery(foodId, ingredientsId), token);
            return Ok(result);
        }

        // Post: api/FoodRecipe
        [HttpPost]
        public async Task<ActionResult<FoodRecipeResponse>> Create
            ([FromBody] FoodRecipeRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new CreateFoodRecipeCommand(request), token);
            return CreatedAtAction(nameof(GetById), new { id = result.IdFoodRecipe }, result);
        }

        // Put: api/FoodRecipe
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FoodRecipeResponse>> Update
            ([FromRoute] Guid id, [FromBody] FoodRecipeRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateFoodRecipeCommand(id, request), token);
            return Ok(result);
        }

        // Delete: api/FoodRecipe/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteFoodRecipeCommand(id), token);
            return result ? NoContent() : NotFound();
        }
    }
}