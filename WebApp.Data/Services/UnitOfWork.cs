using Elect.Data.EF.Interfaces.DbContext;
using Elect.Data.EF.Services.UnitOfWork;

namespace WebApp.Data.Services;

public class UnitOfWork : BaseEntityUnitOfWork
{
    public UnitOfWork(IDbContext dbContext) : base(dbContext)
    {
    }
}
