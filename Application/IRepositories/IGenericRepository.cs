using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IQueryableRepository<T> where T : class
    {
        Task<(IQueryable<T> items, int totalItems)> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1);
        Task<T> GetByCondition(Expression<Func<T, bool>> filter = null, bool traced = true, string? includeProperties = null);

    }
    public interface IModifiableRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);

    }
}
