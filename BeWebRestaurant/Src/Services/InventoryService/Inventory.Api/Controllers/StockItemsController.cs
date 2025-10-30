using Application.Core.DTOs.Requests.Measurement;
using Inventory.Application.DTOs.Requests.StockItems;
using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Application.Modules.StockItems.Commands.CreateStockItems;
using Inventory.Application.Modules.StockItems.Commands.DecreaseStockItems;
using Inventory.Application.Modules.StockItems.Commands.DeleteStockItems;
using Inventory.Application.Modules.StockItems.Commands.IncreaseStockItems;
using Inventory.Application.Modules.StockItems.Commands.UpdateStockItems;
using Inventory.Application.Modules.StockItems.Queries.GetAllStockItems;
using Inventory.Application.Modules.StockItems.Queries.GetStockItemsById;
using Inventory.Application.Modules.StockItems.Queries.GetStockItemsByIngredients;
using Inventory.Application.Modules.StockItems.Queries.GetStockItemsByStatus;
using Inventory.Application.Modules.StockItems.Queries.GetStockItemsByStock;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class StockItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get: api/StockItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockItemsResponse>>> GetAll
            (CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllStockItemsQuery(), token);
            return Ok(result);
        }

        // Get: api/StockItems/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<StockItemsResponse>> GetById
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetStockItemsByIdQuery(id), token);
            return Ok(result);
        }

        // Get: api/StockItems/ingredients/{id}
        [HttpGet("ingredients/{id:guid}")]
        public async Task<ActionResult<IEnumerable<StockItemsResponse>>> GetByIngredients
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetStockItemsByIngredientsQuery(id), token);
            return Ok(result);
        }

        // Get: api/StockItems/stock/{id}
        [HttpGet("stock/{id:guid}")]
        public async Task<ActionResult<IEnumerable<StockItemsResponse>>> GetByStock
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetStockItemsByStockQuery(id), token);
            return Ok(result);
        }

        // Get: api/StockItems/{status}
        [HttpGet("{status}")]
        public async Task<ActionResult<IEnumerable<StockItemsResponse>>> GetByStockItemsStatus
            ([FromRoute] string status, CancellationToken token)
        {
            var result = await _mediator.Send(new GetStockItemsByStatusQuery(status), token);
            return Ok(result);
        }

        // Post: api/StockItems
        [HttpPost]
        public async Task<ActionResult<StockItemsResponse>> Create
            ([FromBody] CreateStockItemsRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new CreateStockItemsCommand(request), token);
            return CreatedAtAction(nameof(GetById), new { id = result.IdStockItems }, result);
        }

        // Put: api/StockItems/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<StockItemsResponse>> Update
            ([FromRoute] Guid id, [FromBody] UpdateStockItemsRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateStockItemsCommand(id, request), token);
            return Ok(result);
        }

        // Delete: api/StockItems/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<bool>> Delete
            ([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteStockItemsCommand(id), token);
            return result ? NoContent() : NotFound();
        }

        // Post: api/StockItems/{id}/decrease
        [HttpPost("{id:guid}/decrease")]
        public async Task<ActionResult<StockItemsResponse>> Decrease
            ([FromRoute] Guid id, [FromBody] MeasurementRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new DecreaseStockItemsCommand(id, request), token);
            return Ok(result);
        }

        // Post: api/StockItems/{id}/increase
        [HttpPost("{id:guid}/increase")]
        public async Task<ActionResult<StockItemsResponse>> Increase
            ([FromRoute] Guid id, [FromBody] MeasurementRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new IncreaseStockItemCommand(id, request), token);
            return Ok(result);
        }
    }
}
