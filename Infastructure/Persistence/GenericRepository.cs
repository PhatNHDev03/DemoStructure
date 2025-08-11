using Application.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Persistence
{
    public class GenericQueryRepository<T> : IQueryableRepository<T> where T : class
    {
        protected   readonly DemoContext _demoContext;
        protected readonly DbSet<T> _dbset;
        public GenericQueryRepository(DemoContext demoContext)
        {
            _demoContext = demoContext;
            _dbset = _demoContext.Set<T>();
        }
        public async Task<(IQueryable<T> items, int totalItems)> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> query = _dbset;
            if (filter != null) query = query.Where(filter);
            int totalItems = await query.CountAsync();
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return (items: query, totalItems: totalItems);
           
        }

        public async Task<T> GetByCondition(Expression<Func<T, bool>> filter = null, bool traced = true, string? includeProperties = null)
        {

            IQueryable<T> query = _dbset;
            if (!traced) query = query.AsNoTracking(); // sử dụng khi chỉ dùng để lấy data 
            if (filter != null) query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            var result = await query.FirstOrDefaultAsync();
            // Nếu vẫn bị track, force detach
            if (result != null && !traced)
            {
                var entry = _demoContext.Entry(result);
                if (entry.State != EntityState.Detached)
                {
                    entry.State = EntityState.Detached;
                }
            }
            return result;
        }
         
    }
    public class GenericCommandRepository<T> : IModifiableRepository<T> where T : class
    {
        protected readonly DemoContext _demoContext;
        protected readonly DbSet<T> _dbset;
        public GenericCommandRepository(DemoContext demoContext)
        {
            _demoContext = demoContext;
            _dbset = _demoContext.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);

            return entity;
        }

        public async Task Delete(T entity)
        {
             _dbset.Remove(entity);
        }

        public async Task<T> Update(T entity)
        {
            AttachIfNeeded(entity);    
            _demoContext.Entry(entity).State = EntityState.Modified; 
            _dbset.Update(entity);
            return  entity;
        }
        private void AttachIfNeeded(T entity) {
            var entityDetached = _demoContext.Entry(entity);
            if (entityDetached.State == EntityState.Detached) _demoContext.Attach(entityDetached);     
        }
    }
}
