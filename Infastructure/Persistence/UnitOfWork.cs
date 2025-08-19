using Application.IRepositories;
using Infastructure.Sql.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Persistence
{
    public class QueryUnitOfWork : IQueryUnitOfWork
    {
        private readonly SqlContext _context;

        public QueryUnitOfWork(SqlContext context)
        {
            _context = context;
        }

        public IQueryableRepository<T> Repository<T>() where T : class
        {
            return new GenericQueryRepository<T>(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
    public class CommandUnitOfWork : ICommandUnitOfWork
    {
        private readonly SqlContext _context;
        private IDbContextTransaction _transaction;
        public CommandUnitOfWork(SqlContext context)
        {
            _context = context;
        }

        public IModifiableRepository<T> Repository<T>() where T : class
        {
            return new GenericCommandRepository<T>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
            return _transaction;
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
