using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Squirrel.Domain.ResultModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Data
{
    public interface IRepositoryContext
    {
        OperationResult OperationResult { get; }
        Task CreateAsync<TEntity>(TEntity item) where TEntity : class;
        Task CreateAsync<TEntity>(TEntity[] items) where TEntity : class;
        Task DeleteAsync<TEntity>(TEntity item) where TEntity : class;
        Task DeleteAsync<TEntity>(TEntity[] items) where TEntity : class;
        Task UpdateAsync<TEntity>(TEntity item) where TEntity : class;
        Task UpdateAsync<TEntity>(TEntity[] items) where TEntity : class;
        Task<TEntity> RetrieveAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<IQueryable<TEntity>> SearchAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
    }
}