using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface ILogService : IBaseService
    {
        Task AddAsync(LogAddModel model);
        Task DeleteAsync(LogRemoveModel model);
        Task<Log> FindByIdAsync(Guid id);
        Task<List<Log>> SearchAsync<TKey>(LogSearchModel model, OrderingModel<Log, TKey> ordering);
        Task<int?> CountAsync(LogSearchModel model);
    }
}
