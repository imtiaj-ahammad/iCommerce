using System.Linq.Expressions;
using eCommerce.Application;

namespace eCommerce.Persistence;

public class GenericQueryRepository<T> : IGenericQueryRepository<T> where T : class
{
    public readonly MssqlDbContext _context;
    public GenericQueryRepository(MssqlDbContext context)
    {
        _context = context;
    }
    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate);
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }
}
