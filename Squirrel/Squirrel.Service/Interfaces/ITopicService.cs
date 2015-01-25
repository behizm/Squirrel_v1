using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface ITopicService : IBaseService
    {
        Task AddAsync(TopicAddModel model);
        Task EditAsync(TopicEditModel model, Guid userId);
        Task DeleteAsync(TopicDeleteModel model, Guid userId);
        Task<Topic> FindByIdAsync(Guid id);
        Task<List<Topic>> SearchAsync(TopicSearchModel model, OrderingModel<Topic> ordering);
        Task<int?> CountAsync(TopicSearchModel model);
        Task PublishAsync(Guid id, Guid userId);
        Task UnPublishAsync(Guid id, Guid userId);
    }
}