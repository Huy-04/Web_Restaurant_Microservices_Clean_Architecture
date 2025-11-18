using MediatR;
using Menu.Application.DTOs.Requests.FoodType;
using Menu.Application.DTOs.Responses.FoodType;
using Menu.Application.Modules.FoodType.Queries.GetAllFoodType;
using Menu.Application.Modules.FoodType.Queries.GetFoodTypeById;
using Menu.Application.Modules.FoodType.Commands.CreateFoodType;
using Menu.Application.Modules.FoodType.Commands.DeleteFoodType;
using Menu.Application.Modules.FoodType.Commands.UpdateFoodType;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class FoodTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FoodTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/FoodType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodTypeResponse>>> GetAll
            (CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllFoodTypeQuery(), token);
            return Ok(result);
        }

        // Get: api/FoodType/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FoodTypeResponse>> GetById
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetFoodTypeByIdQuery(id), token);
            return Ok(result);
        }

        // Post: api/FoodType
        [HttpPost]
        public async Task<ActionResult<FoodTypeResponse>> Create
            ([FromBody] FoodTypeRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new CreateFoodTypeCommand(request), token);
            return CreatedAtAction(nameof(GetById), new { id = result.IdFoodType }, result);
        }

        // Put: api/FoodType/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FoodTypeResponse>> Update
            ([FromRoute] Guid id, [FromBody] FoodTypeRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateFoodTypeCommand(id, request), token);
            return Ok(result);
        }

        // Delete: api/FoodType/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteFoodTypeCommand(id), token);
            return result ? NoContent() : NotFound();
        }
    }
}