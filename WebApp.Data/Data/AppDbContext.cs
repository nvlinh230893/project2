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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.AddConfigFromAssembly<AppDbContext>(typeof(AppDbContext).GetTypeInfo().Assembly);
        builder.DisableCascadingDelete();
        builder.RemovePluralizingTableNameConvention();
        builder.ReplaceTableNameConvention("Entity", string.Empty);
    }
}
