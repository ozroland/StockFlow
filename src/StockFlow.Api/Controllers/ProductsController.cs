using MediatR;

using Microsoft.AspNetCore.Mvc;

using StockFlow.Application.Features.Products.Commands.CreateProduct;
using StockFlow.Application.Features.Products.Commands.UpdateProductStock;
using StockFlow.Application.Features.Products.DTOs;
using StockFlow.Application.Features.Products.Queries.GetProductById;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(ISender mediator) : ControllerBase
{
    // POST api/products
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
    }

    // GET api/products/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);

        var productDto = await mediator.Send(query, cancellationToken);

        return productDto is null ? NotFound() : Ok(productDto);
    }

    // PATCH api/products/{id}/stock
    [HttpPatch("{id:guid}/stock")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateStock(
        [FromRoute] Guid id,
        [FromBody] UpdateProductStockRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProductStockCommand(
            ProductId: id,
            Quantity: request.Quantity,
            ChangeType: request.ChangeType
        );

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
