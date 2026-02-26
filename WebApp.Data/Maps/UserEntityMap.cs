using Elect.Data.EF.Services.Map;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Data.Models;

namespace WebApp.Data.Maps;

public class UserEntityMap : EntityTypeConfiguration<UserEntity>
{
    public override void Map(EntityTypeBuilder<UserEntity> builder)
    {
        base.Map(builder);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Username).IsRequired().HasMaxLength(100);
        builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(500);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.Username).IsUnique();
    }
}
