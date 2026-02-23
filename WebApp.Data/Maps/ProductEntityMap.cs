using Elect.Data.EF.Services.Map;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Data.Models;

namespace WebApp.Data.Maps;

public class ProductEntityMap : EntityTypeConfiguration<ProductEntity>
{
    public override void Map(EntityTypeBuilder<ProductEntity> builder)
    {
        base.Map(builder);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Price).HasPrecision(18, 2);
    }
}
