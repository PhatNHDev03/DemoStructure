using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IQueryUnitOfWork : IDisposable
    {
        IQueryableRepository<T> Repository<T>() where T : class;
    }

    public interface ICommandUnitOfWork : IDisposable
    {
        ICommandRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
        public IDbContextTransaction BeginTransaction();
        public Task RollbackAsync();
        public Task CommitAsync();

    }
}
