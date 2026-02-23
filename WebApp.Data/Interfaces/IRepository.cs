using Elect.Data.EF.Interfaces.Repository;
using Elect.Data.EF.Models;

namespace WebApp.Data.Interfaces;

public interface IRepository<T> : IBaseEntityRepository<T> where T : BaseEntity, new()
{
}
