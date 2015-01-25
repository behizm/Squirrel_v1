using System;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface IConfigService : IBaseService
    {
        Task AddAsync(string key, string value, Guid userId);
        Task EditAsync(string key, string value, Guid userId);
        Task DeleteAsync(string key, Guid userId);
        Task<string> GetAsync(string key);
    }
}