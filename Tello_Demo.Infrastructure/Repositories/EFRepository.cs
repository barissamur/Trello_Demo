using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Tello_Demo.Application.Interfaces;
using Tello_Demo.Infrastructure.Context;

namespace Tello_Demo.Infrastructure.Repositories;

public class EFRepository<T> : RepositoryBase<T>, IReadRepositoryBase<T>, IRepo<T> where T : class
{
    public EFRepository(AppDbContext dbcContext) : base(dbcContext)
    {

    }
}
