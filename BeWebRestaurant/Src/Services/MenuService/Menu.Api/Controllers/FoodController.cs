using MediatR;
using Menu.Application.DTOs.Requests.Food;
using Menu.Application.DTOs.Responses.Food;
using Menu.Application.Modules.Food.Commands.CreateFood;
using Menu.Application.Modules.Food.Commands.DeleteFood;
using Menu.Application.Modules.Food.Commands.UpdateFood;
using Menu.Application.Modules.Food.Queries.GetAllFood;
using Menu.Application.Modules.Food.Queries.GetFoodByFoodType;
using Menu.Application.Modules.Food.Queries.GetFoodById;
using Menu.Application.Modules.Food.Queries.GetFoodByStatus;
using Menu.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class FoodController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FoodController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/Food
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodResponse>>> GetAll
            (CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllFoodQuery(), token);
            return Ok(result);
        }

        // Get: api/Food/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FoodResponse>> GetById
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetFoodByIdQuery(id), token);
            return Ok(result);
        }

        // Get: api/Food/FoodType/{id}
        [HttpGet("foodtype/{id:guid}")]
        public async Task<ActionResult<IEnumerable<FoodResponse>>> GetByFoodType
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetFoodByFoodTypeQuery(id), token);
            return Ok(result);
        }

        // Get: api/Food/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<FoodResponse>>> GetByStatus
            ([FromRoute] FoodStatusEnum status, CancellationToken token)
        {
            var result = await _mediator.Send(new GetFoodByStatusQuery(status), token);
            return Ok(result);
        }

        // Post: api/Food
        [HttpPost]
        public async Task<ActionResult<FoodResponse>> Create
            ([FromBody] CreateFoodRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new CreateFoodCommand(request), token);
            return CreatedAtAction(nameof(GetById), new { id = result.IdFood }, result);
        }

        // Put: api/Food/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FoodResponse>> Update
            ([FromRoute] Guid id, UpdateFoodRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateFoodCommand(id, request), token);
            return Ok(result);
        }

        // Delete: api/Food/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<bool>> Delete
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteFoodCommand(id), token);
            return result ? NoContent() : NotFound();
        }
    }
}
