using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface IFileService:IBaseService
    {
        Task AddAsync(File file);
        Task EditAsync(File file);
        Task RemoveAsync(Guid fileId, Guid userId);
        Task<File> FindByIdAsync(Guid fileId);
        Task<List<File>> SearchAsync(FileSearchModel model, OrderingModel<File> ordering);
        Task<int?> CountAsync(FileSearchModel model);

    }
}