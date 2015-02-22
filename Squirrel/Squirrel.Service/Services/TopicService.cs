using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ExtensionMethods;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

namespace Squirrel.Service.Services
{
    class TopicService : BaseService, ITopicService
    {
        public async Task AddAsync(TopicAddModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Title))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            if (string.IsNullOrEmpty(model.Username))
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            model.Username = model.Username.ToLower();

            var task1 = RepositoryContext.RetrieveAsync<User>(x => x.Username.ToLower() == model.Username);
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
                PostsOrdering = model.PostsOrdering ?? PostsOrdering.Newer,
                Title = model.Title,
                OwnerId = user.Id,
                EditDate = DateTime.Now,
            };
            await RepositoryContext.CreateAsync(item);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task EditAsync(TopicEditModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Title))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            if (string.IsNullOrEmpty(model.Username))
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            model.Username = model.Username.ToLower();

            var isChanged = false;

            var taskUser = RepositoryContext.RetrieveAsync<User>(x => x.Username.ToLower() == model.Username);
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

            if (!user.IsAdmin && user.Id != topic.OwnerId)
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
            topic.PostsOrdering = model.PostsOrdering ?? PostsOrdering.Newer;
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

            if (!user.IsAdmin && user.Id != topic.OwnerId)
            {
                Result = OperationResult.Failed(ServiceMessages.TopicService_NoAccess);
                return;
            }

            // todo: بررسی ارتباطها

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

        public async Task<List<Topic>> SearchAsync<TKey>(TopicSearchModel model, OrderingModel<Topic, TKey> ordering)
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
                        (string.IsNullOrEmpty(model.Username) || x.Owner.Username.ToLower() == model.Username.ToLower()) &&
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
                        await
                            items
                                .OrderBy(ordering.OrderByKeySelector)
                                .Skip(ordering.Skip)
                                .Take(ordering.Take)
                                .ToListAsync();
                }
                return
                    await
                        items
                            .OrderByDescending(ordering.OrderByKeySelector)
                            .Skip(ordering.Skip)
                            .Take(ordering.Take)
                            .ToListAsync();
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
                        (string.IsNullOrEmpty(model.Username) || x.Owner.Username.ToLower() == model.Username.ToLower()) &&
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

        public async Task PublishAsync(Guid id, string username)
        {
            await ChangePublishAsync(id, username, true);
        }

        public async Task UnPublishAsync(Guid id, Guid userId)
        {
            await ChangePublishAsync(id, userId, false);
        }

        public async Task UnPublishAsync(Guid id, string username)
        {
            await ChangePublishAsync(id, username, false);
        }

        public async Task<List<Post>> Posts(Guid id)
        {
            var topic = await RepositoryContext.RetrieveAsync<Topic>(x => x.Id == id);
            if (topic == null)
            {
                Result = OperationResult.Failed(ServiceMessages.TopicService_TopicNotFound);
                return null;
            }

            if (topic.Posts == null || !topic.Posts.Any())
            {
                Result = OperationResult.Failed(ServiceMessages.TopicService_TopicHasNoPost);
                return null;
            }

            switch (topic.PostsOrdering)
            {
                case PostsOrdering.LastEdited:
                    return topic.Posts.OrderByDescending(x => x.EditDate.HasValue ? x.EditDate.Value : x.CreateDate).ToList();

                case PostsOrdering.Newer:
                    return topic.Posts.OrderByDescending(x => x.CreateDate).ToList();

                case PostsOrdering.Older:
                    return topic.Posts.OrderBy(x => x.CreateDate).ToList();

                case PostsOrdering.Popular:
                    return topic.Posts.OrderByDescending(x => x.Votes.Summery()).ToList();

                default:
                    return topic.Posts.ToList();
            }
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

            if (!user.IsAdmin && user.Id != topic.OwnerId)
            {
                Result = OperationResult.Failed(ServiceMessages.TopicService_NoAccess);
                return;
            }

            if (topic.IsPublished == publishState)
            {
                Result = OperationResult.Success;
                return;
            }

            if (publishState)
            {
                if (topic.Posts == null || !topic.Posts.Any())
                {
                    Result = OperationResult.Failed(ServiceMessages.TopicService_NoPostToPublish);
                    return;
                }
                if (topic.Posts.All(p => !p.IsPublic))
                {
                    Result = OperationResult.Failed(ServiceMessages.TopicService_NoPublicPostToPublish);
                    return;
                }
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

        private async Task ChangePublishAsync(Guid id, string username, bool publishState)
        {
            if (string.IsNullOrEmpty(username))
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            username = username.ToLower();

            var taskUser = RepositoryContext.RetrieveAsync<User>(x => x.Username.ToLower() == username);
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

            if (!user.IsAdmin && user.Id != topic.OwnerId)
            {
                Result = OperationResult.Failed(ServiceMessages.TopicService_NoAccess);
                return;
            }

            if (topic.IsPublished == publishState)
            {
                Result = OperationResult.Success;
                return;
            }

            if (publishState)
            {
                if (topic.Posts == null || !topic.Posts.Any())
                {
                    Result = OperationResult.Failed(ServiceMessages.TopicService_NoPostToPublish);
                    return;
                }
                if (topic.Posts.All(p => !p.IsPublic))
                {
                    Result = OperationResult.Failed(ServiceMessages.TopicService_NoPublicPostToPublish);
                    return;
                }
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
