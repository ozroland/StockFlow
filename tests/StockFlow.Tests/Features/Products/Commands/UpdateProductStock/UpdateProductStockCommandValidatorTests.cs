using FluentValidation.TestHelper;

using StockFlow.Application.Features.Products.Commands.UpdateProductStock;

namespace StockFlow.Tests.Features.Products.Commands.UpdateProductStock;

public class UpdateProductStockCommandValidatorTests
{
    private readonly UpdateProductStockCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_ProductId_Is_Empty()
    {
        // Arrange
        var command = new UpdateProductStockCommand(Guid.Empty, 10, StockChangeType.Increase);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Should_Have_Error_When_Quantity_Is_Zero_Or_Negative(int quantity)
    {
        // Arrange
        var command = new UpdateProductStockCommand(Guid.NewGuid(), quantity, StockChangeType.Increase);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Quantity).WithErrorMessage("Quantity must be greater than 0.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        var command = new UpdateProductStockCommand(Guid.NewGuid(), 10, StockChangeType.Increase);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
