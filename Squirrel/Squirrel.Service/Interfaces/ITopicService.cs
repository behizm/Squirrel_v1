using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface ITopicService : IBaseService
    {
        Task AddAsync(TopicAddModel model);
        Task EditAsync(TopicEditModel model);
        Task DeleteAsync(TopicDeleteModel model, Guid userId);
        Task<Topic> FindByIdAsync(Guid id);
        Task<List<Topic>> SearchAsync<TKey>(TopicSearchModel model, OrderingModel<Topic, TKey> ordering);
        Task<int?> CountAsync(TopicSearchModel model);
        Task<List<Topic>> SearchInPublishedAsync<TKey>(TopicPublishedSearchModel model, OrderingModel<Topic, TKey> ordering);
        Task<int?> CountInPublishedAsync(TopicPublishedSearchModel model);
        Task PublishAsync(Guid id, Guid userId);
        Task PublishAsync(Guid id, string username);
        Task UnPublishAsync(Guid id, Guid userId);
        Task UnPublishAsync(Guid id, string username);
        Task<List<Post>> Posts(Guid id);
    }
}