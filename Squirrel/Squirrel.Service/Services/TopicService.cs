using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task DeleteAsync(TopicDeleteModel model, Guid userId)
        {
            if (model == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

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

            await RepositoryContext.DeleteAsync(topic);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task<Topic> FindByIdAsync(Guid id)
        {
            var item = await RepositoryContext.RetrieveAsync<Topic>(x => x.Id == id);
            if (item == null)
            {
                Result = OperationResult.Failed(ServiceMessages.TopicService_TopicNotFound);
                return null;
            }
            Result = OperationResult.Success;
            return item;
        }

        public async Task<List<Topic>> SearchAsync(TopicSearchModel model, OrderingModel<Topic> ordering)
        {
            if (model == null || ordering == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var items =
                await
                    RepositoryContext.SearchAsync<Topic>(x =>
                        (string.IsNullOrEmpty(model.Title) || x.Title.Contains(model.Title)) &&
                        (string.IsNullOrEmpty(model.Category) || x.Category.Name.Contains(model.Title)) &&
                        (string.IsNullOrEmpty(model.Username) || x.User.Username == model.Username) &&
                        (!model.IsPublished.HasValue || x.IsPublished == model.IsPublished) &&
                        (!model.PostsOrdering.HasValue || x.PostsOrdering == model.PostsOrdering));

            if (items == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            try
            {
                Result = OperationResult.Success;
                if (ordering.IsAscending)
                {
                    return
                        await items.OrderBy(ordering.KeySelector).Skip(ordering.Skip).Take(ordering.Take).ToListAsync();
                }
                return
                        await items.OrderByDescending(ordering.KeySelector).Skip(ordering.Skip).Take(ordering.Take).ToListAsync();
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }
        }

        public async Task<int?> CountAsync(TopicSearchModel model)
        {
            if (model == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var count =
                await
                    RepositoryContext.CountAsync<Topic>(x =>
                        (string.IsNullOrEmpty(model.Title) || x.Title.Contains(model.Title)) &&
                        (string.IsNullOrEmpty(model.Category) || x.Category.Name.Contains(model.Title)) &&
                        (string.IsNullOrEmpty(model.Username) || x.User.Username == model.Username) &&
                        (!model.IsPublished.HasValue || x.IsPublished == model.IsPublished) &&
                        (!model.PostsOrdering.HasValue || x.PostsOrdering == model.PostsOrdering));

            if (count == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return count;
        }

        public async Task PublishAsync(Guid id, Guid userId)
        {
            await ChangePublishAsync(id, userId, true);
        }

        public async Task UnPublishAsync(Guid id, Guid userId)
        {
            await ChangePublishAsync(id, userId, false);
        }


        private async Task ChangePublishAsync(Guid id, Guid userId, bool publishState)
        {
            var taskUser = RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            var taskTopic = RepositoryContext.RetrieveAsync<Topic>(x => x.Id == id);

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

            if (topic.IsPublished == publishState)
            {
                Result = OperationResult.Success;
                return;
            }

            topic.IsPublished = publishState;
            topic.EditDate = DateTime.Now;
            await RepositoryContext.UpdateAsync(topic);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }
    }
}
