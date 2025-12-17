using MediatR;

using Microsoft.AspNetCore.Mvc;

using StockFlow.Application.Features.Products.Commands.CreateProduct;
using StockFlow.Application.Features.Products.Queries.GetProductById;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(ISender mediator) : ControllerBase
{
    // POST api/products
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        var id = await mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    // GET api/products/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await mediator.Send(query);

        return product is null ? NotFound() : Ok(product);
    }
}