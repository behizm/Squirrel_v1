using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Squirrel.Domain.ResultModels;

namespace Squirrel.Data
{
    public interface IWarehouseContext
    {
        OperationResult OperationResult { get; }
        void Add<TEntity>(TEntity item) where TEntity : class;
        void Add<TEntity>(TEntity[] items) where TEntity : class;
        void Remove<TEntity>(TEntity item) where TEntity : class;
        void Remove<TEntity>(TEntity[] items) where TEntity : class;
        void Modify<TEntity>(TEntity item) where TEntity : class;
        void Modify<TEntity>(TEntity[] items) where TEntity : class;
        Task<TEntity> RetrieveAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<IQueryable<TEntity>> SearchAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<OperationResult> SaveChangesAsync();
        OperationResult SaveChanges();
    }
}