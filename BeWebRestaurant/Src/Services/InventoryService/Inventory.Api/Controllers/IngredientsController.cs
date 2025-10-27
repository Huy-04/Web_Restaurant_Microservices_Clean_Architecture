using Inventory.Application.DTOs.Requests.Ingredients;
using Inventory.Application.DTOs.Responses.Ingredients;
using Inventory.Application.Modules.Ingredients.Commands.CreateIngredients;
using Inventory.Application.Modules.Ingredients.Commands.DeleteIngredients;
using Inventory.Application.Modules.Ingredients.Commands.UpdateIngredients;
using Inventory.Application.Modules.Ingredients.Queries.GetAll;
using Inventory.Application.Modules.Ingredients.Queries.GetIngredientsById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class IngredientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IngredientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/Ingredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientsResponse>>> GetAll
            (CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllIngredientsQuery(), token);
            return Ok(result);
        }

        // Get: api/Ingredients/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IngredientsResponse>> GetById
            (Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetIngredientsByIdQuery(id), token);
            return Ok(result);
        }

        // Post: api/Ingredients
        [HttpPost]
        public async Task<ActionResult<IngredientsResponse>> Create
            ([FromBody] IngredientsRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new CreateIngredientsCommand(request), token);
            return CreatedAtAction(nameof(GetById), new { id = result.IdIngredients }, result);
        }

        // Put: api/Ingredients/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<IngredientsResponse>> Update
            ([FromRoute] Guid id, [FromBody] IngredientsRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateIngredientsCommand(id, request), token);
            return Ok(result);
        }

        // Delete: api/Ingredients/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteIngredientsCommand(id), token);
            return result ? NoContent() : NotFound();
        }
    }
}