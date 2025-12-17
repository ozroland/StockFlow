using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StockFlow.Domain.Entities;
using StockFlow.Domain.ValueObjects;

namespace StockFlow.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Sku)
            .HasConversion(
                sku => sku.Value,
                value => Sku.Create(value)!
            )
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(p => p.Sku).IsUnique();

        builder.OwnsOne(p => p.Price, priceBuilder =>
        {
            priceBuilder.Property(m => m.Amount)
                .HasColumnName("PriceAmount")
                .HasPrecision(18, 2);

            priceBuilder.Property(m => m.Currency)
                .HasColumnName("PriceCurrency")
                .HasMaxLength(3);
        });

        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.StockLevel).IsRequired();
    }
}