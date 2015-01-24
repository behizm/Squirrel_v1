using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface ITagService
    {
        OperationResult Result { get; }

        Task<List<string>> SearchAsync(TagSearchModel model, OrderingModel<Tag> ordering);
        Task<int?> CountAsync(TagSearchModel model);
        Task DeleteAsync(Guid tagId, Guid userId);
    }
}