using System.Linq.Expressions;

namespace Infrastructure.Repositories.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllQuerable();
        Task<T?> GetAsync(int id);
        T? Get(int id);
        Task<T> Add(T entity);
        T AddSync(T entity);
        T UpdateSync(T entity);
        IQueryable<T> DeleteAll();
        Task<int> AddRangeAsync(ICollection<T> t);
        Task<int> UpdateRangeAsync(ICollection<T> fields);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
        Task<int> DeleteAsync(T t);
        Task<bool> Exist(Expression<Func<T, bool>> expression);

    }
}