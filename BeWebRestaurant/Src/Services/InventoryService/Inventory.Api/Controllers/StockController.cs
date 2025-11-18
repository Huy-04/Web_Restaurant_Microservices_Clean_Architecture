using Inventory.Application.DTOs.Requests.Stock;
using Inventory.Application.DTOs.Responses.Stock;
using Inventory.Application.Modules.Stock.Commands.CreateStock;
using Inventory.Application.Modules.Stock.Commands.DeleteStock;
using Inventory.Application.Modules.Stock.Commands.UpdateStock;
using Inventory.Application.Modules.Stock.Queries.GetAllStock;
using Inventory.Application.Modules.Stock.Queries.GetStockById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class StockController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/Stock
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockResponse>>> GetAll
            (CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllStockQuery(), token);
            return Ok(result);
        }

        // Get: api/Stock/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<StockResponse>> GetById
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetStockByIdQuery(id), token);
            return Ok(result);
        }

        // Post: api/Stock
        [HttpPost]
        public async Task<ActionResult<StockResponse>> Create
            ([FromBody] StockRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new CreateStockCommand(request), token);
            return CreatedAtAction(nameof(GetById), new { id = result.IdStock }, result);
        }

        // Put: api/Stock/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<StockResponse>> Update
            ([FromRoute] Guid id, [FromBody] StockRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateStockCommand(id, request), token);
            return Ok(result);
        }

        // Delete: api/Stock/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteStockCommand(id), token);
            return result ? NoContent() : NotFound();
        }
    }
}