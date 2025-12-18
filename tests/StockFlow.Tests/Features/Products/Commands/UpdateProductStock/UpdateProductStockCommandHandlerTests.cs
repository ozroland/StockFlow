using NSubstitute;
using StockFlow.Application.Common.Interfaces;
using StockFlow.Application.Features.Products.Commands.UpdateProductStock;
using StockFlow.Domain.Common;
using StockFlow.Domain.Entities;
using StockFlow.Domain.Exceptions;
using StockFlow.Domain.ValueObjects;

namespace StockFlow.Tests.Features.Products.Commands.UpdateProductStock;

public class UpdateProductStockCommandHandlerTests
{
    private readonly IProductRepository _repositoryMock;
    private readonly UpdateProductStockCommandHandler _handler;

    public UpdateProductStockCommandHandlerTests()
    {
        _repositoryMock = Substitute.For<IProductRepository>();
        _handler = new UpdateProductStockCommandHandler(_repositoryMock);
    }

    [Fact]
    public async Task Handle_Should_Throw_KeyNotFoundException_When_Product_Not_Found()
    {
        // Arrange
        var command = new UpdateProductStockCommand(Guid.NewGuid(), 10, StockChangeType.Increase);

        _repositoryMock.GetByIdAsync(command.ProductId, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_Throw_InsufficientStockException_When_Stock_Is_Too_Low()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var product = CreateTestProduct(productId, 5);

        _repositoryMock.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(product);

        var command = new UpdateProductStockCommand(productId, 10, StockChangeType.Decrease);

        // Act & Assert
        await Assert.ThrowsAsync<InsufficientStockException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_Update_Stock_Success()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = CreateTestProduct(productId, 10);

        _repositoryMock.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(product);

        var command = new UpdateProductStockCommand(productId, 5, StockChangeType.Increase);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(15, product.StockLevel);

        await _repositoryMock.Received(1).UpdateAsync(product, Arg.Any<CancellationToken>());
        await _repositoryMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private static Product CreateTestProduct(Guid id, int initialStock)
    {
        var sku = Sku.Create("SKUA-1234")!;
        var price = new Money(100, "EUR");
        var product = new Product(sku, "Test Product", price);

        typeof(BaseEntity)
        .GetProperty("Id")?
        .SetValue(product, id);

        if (initialStock > 0)
            product.AddStock(initialStock);

        return product;
    }
}
