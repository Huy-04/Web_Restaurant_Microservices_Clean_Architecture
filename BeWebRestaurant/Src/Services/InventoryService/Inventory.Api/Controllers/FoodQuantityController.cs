using Inventory.Application.DTOs.Responses.FoodQuantity;
using Inventory.Application.Modules.FoodQuantity.Queries.GetFoodQuantity;
using Inventory.Application.Modules.FoodQuantity.Queries.GetFoodQuantityById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class FoodQuantityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FoodQuantityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/FoodQuantity
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<FoodQuantityResponse>>> GetAll
            (CancellationToken token)
        {
            var result = await _mediator.Send(new GetFoodQuantityQuery(), token);
            return Ok(result);
        }

        // Get: api/FoodQuantity/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FoodQuantityResponse>> GetById
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetFoodQuantityByIdQuery(id), token);
            return Ok(result);
        }
    }
}