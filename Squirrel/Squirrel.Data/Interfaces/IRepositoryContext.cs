using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Squirrel.Domain.ResultModels;

namespace Squirrel.Data
{
    public interface IRepositoryContext
    {
        OperationResult OperationResult { get; }
        Task AddAsync<TEntity>(TEntity item) where TEntity : class;
        Task AddAsync<TEntity>(TEntity[] items) where TEntity : class;
        Task RemoveAsync<TEntity>(TEntity item) where TEntity : class;
        Task RemoveAsync<TEntity>(TEntity[] items) where TEntity : class;
        Task ModifyAsync<TEntity>(TEntity item) where TEntity : class;
        Task ModifyAsync<TEntity>(TEntity[] items) where TEntity : class;
        Task<TEntity> RetrieveAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<IQueryable<TEntity>> SearchAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
    }
}