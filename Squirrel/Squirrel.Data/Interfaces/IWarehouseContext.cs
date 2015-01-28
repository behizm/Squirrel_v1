using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Squirrel.Domain.ResultModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Data
{
    public interface IWarehouseContext
    {
        OperationResult OperationResult { get; }
        void Create<TEntity>(TEntity item) where TEntity : class;
        void Create<TEntity>(TEntity[] items) where TEntity : class;
        void Delete<TEntity>(TEntity item) where TEntity : class;
        void Delete<TEntity>(TEntity[] items) where TEntity : class;
        void Update<TEntity>(TEntity item) where TEntity : class;
        void Update<TEntity>(TEntity[] items) where TEntity : class;
        Task<TEntity> RetrieveAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<IQueryable<TEntity>> SearchAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<int?> CountAsync<TEntity>() where TEntity : class;
        Task<int?> CountAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<OperationResult> SaveChangesAsync();
        OperationResult SaveChanges();
    }
}