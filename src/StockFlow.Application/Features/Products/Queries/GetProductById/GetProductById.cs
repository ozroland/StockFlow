using AutoMapper;

using MediatR;

using StockFlow.Application.Common.Interfaces;
using StockFlow.Application.Features.Products.DTOs;

namespace StockFlow.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;

public class GetProductByIdQueryHandler(IProductRepository repository, IMapper mapper)
    : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);

        return product is null ? null : mapper.Map<ProductDto>(product);
    }
}
