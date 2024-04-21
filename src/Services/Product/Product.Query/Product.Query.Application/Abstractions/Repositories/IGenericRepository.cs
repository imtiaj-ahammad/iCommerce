using System.Linq.Expressions;

namespace Product.Query.Application;

public interface IGenericRepository <T> where T : class
{
	T GetById(int id);
	IEnumerable<T> GetAll();
	IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
}