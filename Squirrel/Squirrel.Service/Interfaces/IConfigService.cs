using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface IConfigService : IBaseService
    {
        Task AddAsync(ConfigAddModel model, string username);
        Task EditAsync(ConfigEditModel model, string username);
        Task DeleteAsync(Guid id, string username);
        Task<string> GetValueAsync(string key);
        Task<Config> FindByIdAsync(Guid id);
        Task<Config> FindByKeyAsync(string key);
        Task<List<Config>> SearchAsync(string key, OrderingModel<Config> ordering);
        Task<int?> CountAsync(string key);
    }
}