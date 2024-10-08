using System.Linq.Expressions;
using Domain;

namespace DataLayer.Interface;

public interface IAbstractRepository<T> where T : BaseEntity
{
    Task<T?> GetById(int id);
    Task<T?> FirstOrDefault(Expression<Func<T?, bool>> predicate);

    Task Add(T? entity);
    Task<int> Update(T entity);
    Task<int> Remove(T? entity);

    Task<IEnumerable<T?>> GetAll();
    Task<IEnumerable<T?>> GetWhere(Expression<Func<T?, bool>> predicate);

    Task<int> CountAll();
    Task<int> CountWhere(Expression<Func<T?, bool>> predicate);
}