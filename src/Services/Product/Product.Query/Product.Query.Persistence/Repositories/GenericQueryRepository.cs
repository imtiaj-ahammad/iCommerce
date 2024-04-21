using System.Linq.Expressions;
using Product.Query.Application;

namespace Product.Query.Persistence;

public class GenericQueryRepository<T> : IGenericRepository<T> where T : class
{
    public readonly MongoDbContext _context;
    public GenericQueryRepository(MongoDbContext context)
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