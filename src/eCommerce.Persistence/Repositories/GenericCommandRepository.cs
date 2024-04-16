using eCommerce.Application;

namespace eCommerce.Persistence;

public class GenericCommandRepository<T> : IGenericCommandRepository<T> where T : class
{
    public readonly MssqlDbContext _context;
    public GenericCommandRepository(MssqlDbContext context)
    {
         _context = context;
    }
    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }
    public void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }
    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }
}
