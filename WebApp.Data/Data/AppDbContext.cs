using System.Reflection;
using Elect.Data.EF.Utils.ModelBuilderUtils;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Models;

namespace WebApp.Data.Data;

public class AppDbContext : Elect.Data.EF.Services.DbContext.DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<UserFBEntity> UserFBs => Set<UserFBEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.AddConfigFromAssembly<AppDbContext>(typeof(AppDbContext).GetTypeInfo().Assembly);
        builder.DisableCascadingDelete();
        builder.RemovePluralizingTableNameConvention();
        builder.ReplaceTableNameConvention("Entity", string.Empty);

        // Seed sample users (password: 123456)
        var passwordHash = "$2a$11$9ly0cGZ8Gd3rnbCbHYV6CO6KwfWszlLjTQ5CWnpXnNvypI1OxvKte";
        var now = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

        builder.Entity<UserEntity>().HasData(
            new UserEntity
            {
                Id = 1,
                GlobalId = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000001"),
                Email = "admin@webapp.com",
                Username = "admin",
                PasswordHash = passwordHash,
                CreatedTime = now,
                LastUpdatedTime = now
            },
            new UserEntity
            {
                Id = 2,
                GlobalId = Guid.Parse("a1b2c3d4-0002-0000-0000-000000000002"),
                Email = "user@webapp.com",
                Username = "user",
                PasswordHash = passwordHash,
                CreatedTime = now,
                LastUpdatedTime = now
            }
        );
    }
}
