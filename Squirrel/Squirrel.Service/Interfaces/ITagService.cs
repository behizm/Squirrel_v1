using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface ITagService : IBaseService
    {
        Task<List<string>> SearchAsync(TagSearchModel model, OrderingModel<Tag> ordering);
        Task<int?> CountAsync(TagSearchModel model);
        Task DeleteAsync(Guid tagId, Guid userId);
        Task<ListModel<Topic>> PublishedTopicsAsync(string tagName, int skip, int take);
        Task<ListModel<TagWeightModel>> TagsWithWeightAsync<TKey>(OrderingModel<TagWeightModel, TKey> ordering, bool isPublished = false);
    }
}