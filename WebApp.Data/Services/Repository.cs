using Elect.Data.EF.Interfaces.DbContext;
using Elect.Data.EF.Models;
using Elect.Data.EF.Services.Repository;
using WebApp.Data.Interfaces;

namespace WebApp.Data.Services;

public class Repository<T> : BaseEntityRepository<T>, IRepository<T> where T : BaseEntity, new()
{
    public Repository(IDbContext dbContext) : base(dbContext)
    {
    }
}
