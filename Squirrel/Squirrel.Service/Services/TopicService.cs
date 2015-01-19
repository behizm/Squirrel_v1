using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squirrel.Data;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

namespace Squirrel.Service.Services
{
    class TopicService : ITopicService
    {
        private IRepositoryContext _repositoryContext;
        private IRepositoryContext RepositoryContext
        {
            get { return _repositoryContext ?? (_repositoryContext = DataIOC.Get<IRepositoryContext>()); }
        }

        public OperationResult Result { get; private set; }


        public async Task AddAsync(TopicAddModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Title))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var task1 = RepositoryContext.RetrieveAsync<User>(x => x.Id == model.UserId);
            var task2 = RepositoryContext.RetrieveAsync<Category>(x => x.Id == model.CategoryId);

            var user = await task1;
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            var category = await task2;
            if (category == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                return;
            }

            var item = new Topic
            {
                CategoryId = category.Id,
                IsPublished = false,
                PostsOrdering = model.PostsOrdering,
                Title = model.Title,
                UserId = user.Id,
            };
            await RepositoryContext.CreateAsync(item);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task EditAsync(TopicEditModel model, Guid userId)
        {
            if (model == null || string.IsNullOrEmpty(model.Title))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var isChanged = false;

            var taskUser = RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            var taskTopic = RepositoryContext.RetrieveAsync<Topic>(x => x.Id == model.Id);

            var user = await taskUser;
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            var topic = await taskTopic;
            if (topic == null)
            {
                Result = OperationResult.Failed(ServiceMessages.TopicService_TopicNotFound);
                return;
            }

            if (!user.IsAdmin && user.Id != topic.UserId)
            {
                Result = OperationResult.Failed(ServiceMessages.TopicService_NoAccess);
                return;
            }

            if (model.CategoryId != topic.CategoryId)
            {
                var category = await RepositoryContext.RetrieveAsync<Category>(x => x.Id == model.CategoryId);
                if (category == null)
                {
                    Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                    return;
                }
                topic.CategoryId = category.Id;
                isChanged = true;
            }

            if (model.Title == topic.Title && model.PostsOrdering == topic.PostsOrdering && !isChanged)
            {
                Result = OperationResult.Success;
                return;
            }

            topic.Title = model.Title;
            topic.PostsOrdering = model.PostsOrdering;
            topic.EditDate = DateTime.Now;
            await RepositoryContext.UpdateAsync(topic);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public Task DeleteAsync(TopicDeleteModel model, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Topic> FindByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Topic>> SearchAsync(TopicSearchModel model, OrderingModel<Topic> ordering)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(TopicSearchModel model)
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync(Guid id, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UnPublishAsync(Guid id, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
