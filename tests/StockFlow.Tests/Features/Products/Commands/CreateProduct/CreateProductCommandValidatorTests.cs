using FluentValidation.TestHelper;

using StockFlow.Application.Features.Products.Commands.CreateProduct;

namespace StockFlow.Tests.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Sku_Is_Empty()
    {
        // Arrange
        var command = new CreateProductCommand("", "Valid Name", 100, "EUR", null);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Sku);
    }

    [Fact]
    public void Should_Have_Error_When_Price_Is_Negative()
    {
        // Arrange
        var command = new CreateProductCommand("SKU-123", "Valid Name", -500, "EUR", null);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PriceAmount).WithErrorMessage("Price must be greater than zero.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateProductCommand("SKU-123", "iPhone 15", 1000, "EUR", "Description");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
