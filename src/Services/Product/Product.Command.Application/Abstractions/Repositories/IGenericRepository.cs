namespace Product.Command.Application;

public interface IGenericRepository<T> where T : class
{
	void Add(T entity);
	void AddRange(IEnumerable<T> entities);
	void Remove(T entity);
	void RemoveRange(IEnumerable<T> entities);
}