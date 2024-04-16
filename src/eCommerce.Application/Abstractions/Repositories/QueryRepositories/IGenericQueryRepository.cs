using System.Linq.Expressions;

namespace eCommerce.Application;

public interface IGenericQueryRepository<T> where T : class
{
		T GetById(int id);
		IEnumerable<T> GetAll();
		IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
}
