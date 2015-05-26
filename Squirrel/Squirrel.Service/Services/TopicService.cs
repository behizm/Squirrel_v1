using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ExtensionMethods;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;
using Squirrel.Utility.Helpers;

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

            var issueId = await GenerateIssueId();
            if (issueId.IsNothing())
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
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
                IssueId = issueId,
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

            if (model.Title == topic.Title && model.PostsOrdering == topic.PostsOrdering &&
                !isChanged && topic.PublishDate == model.PublishDateTime)
            {
                Result = OperationResult.Success;
                return;
            }

            topic.Title = model.Title;
            topic.PostsOrdering = model.PostsOrdering ?? PostsOrdering.Newer;
            topic.PublishDate = model.PublishDateTime;
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

            var taskTopic = WarehouseContext.RetrieveAsync<Topic>(x => x.Id == model.Id);
            var taskUser = WarehouseContext2.RetrieveAsync<User>(x => x.Id == userId);

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

            topic.Posts.ForEach(p =>
            {
                if (p.Comments != null && p.Comments.Any())
                    WarehouseContext.Delete(p.Comments.ToArray());

                if (p.Votes != null && p.Votes.Any())
                    WarehouseContext.Delete(p.Votes.ToArray());
            });
            WarehouseContext.Delete(topic.Posts.ToArray());
            WarehouseContext.Delete(topic);
            await WarehouseContext.SaveChangesAsync();
            if (WarehouseContext.OperationResult.Succeeded)
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

        public async Task<Topic> FindByIssueIdAsync(string issueId)
        {
            var item = await RepositoryContext.RetrieveAsync<Topic>(x => x.IssueId == issueId);
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

            var items = await RepositoryContext.SearchAsync(GetSearchExpression(model));

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

            var count = await RepositoryContext.CountAsync(GetSearchExpression(model));

            if (count == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return count;
        }

        public async Task<List<Topic>> SearchInPublishedAsync<TKey>(TopicPublishedSearchModel model, OrderingModel<Topic, TKey> ordering)
        {
            if (model == null || ordering == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var items = await RepositoryContext.SearchAsync(await GetSearchInPublishedExpression(model));
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

        public async Task<int?> CountInPublishedAsync(TopicPublishedSearchModel model)
        {
            if (model == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var count = await RepositoryContext.CountAsync(await GetSearchInPublishedExpression(model));
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

                case PostsOrdering.NewerPublish:
                    return topic.Posts.OrderByDescending(x => x.PublishDate.HasValue ? x.PublishDate.Value : x.CreateDate).ToList();

                case PostsOrdering.OlderPublish:
                    return topic.Posts.OrderBy(x => x.PublishDate.HasValue ? x.PublishDate.Value : x.CreateDate).ToList();

                default:
                    return topic.Posts.ToList();
            }
        }

        public async Task PlusView(Guid id)
        {
            var topic = await RepositoryContext.RetrieveAsync<Topic>(x => x.Id == id);
            if (topic == null)
            {
                Result = OperationResult.Failed(ServiceMessages.TopicService_TopicNotFound);
                return;
            }

            topic.View = topic.View + 1;
            await RepositoryContext.UpdateAsync(topic);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
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
            if (publishState && !topic.PublishDate.HasValue)
            {
                topic.PublishDate = DateTime.Now;
            }
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
            if (publishState && !topic.PublishDate.HasValue)
            {
                topic.PublishDate = DateTime.Now;
            }
            await RepositoryContext.UpdateAsync(topic);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        private static Expression<Func<Topic, bool>> GetSearchExpression(TopicSearchModel model)
        {
            model.Username = model.Username.IsNotNothing() ? model.Username.TrimAndLower() : string.Empty;

            return x =>
                (string.IsNullOrEmpty(model.Title) || x.Title.Contains(model.Title)) &&
                (string.IsNullOrEmpty(model.Category) || x.Category.Name.Contains(model.Category)) &&
                (string.IsNullOrEmpty(model.Username) || x.Owner.Username.ToLower() == model.Username) &&
                (!model.IsPublished.HasValue || x.IsPublished == model.IsPublished) &&
                (!model.PostsOrdering.HasValue || x.PostsOrdering == model.PostsOrdering) &&
                (!model.PublishDateFrom.HasValue || x.PublishDate >= model.PublishDateFrom) &&
                (!model.PublishDateTo.HasValue || x.PublishDate <= model.PublishDateTo);
        }

        private async Task<Expression<Func<Topic, bool>>> GetSearchInPublishedExpression(TopicPublishedSearchModel model)
        {
            var textList = new List<string>();
            if (model.SeachText.IsNotNothing())
            {
                model.SeachText = model.SeachText.Trim();
                while (model.SeachText.Contains("  "))
                {
                    model.SeachText = model.SeachText.Replace("  ", " ");
                }
                textList = model.SeachText.Split(' ').ToList();
            }

            model.Author = model.Author.IsNotNothing() ? model.Author.TrimAndLower() : string.Empty;
            model.Category = model.Category.IsNotNothing() ? model.Category.TrimAndLower() : string.Empty;

            List<Guid> catFamilyIds;
            if (model.CategoryId.HasValue)
            {
                catFamilyIds = await GetCategoryFamilyById(model.CategoryId.Value);
            }
            else if (model.Category.IsNotNothing())
            {
                catFamilyIds = await GetCategoryFamilyByName(model.Category);
            }
            else
            {
                model.CategoryId = null;
                catFamilyIds = new List<Guid>();
            }

            if (!model.PublishDateTo.HasValue || model.PublishDateTo > DateTime.Now)
            {
                model.PublishDateTo = DateTime.Now;
            }

            Expression<Func<Topic, bool>> expression =
                x =>
                    (x.IsPublished) && (x.PublishDate <= model.PublishDateTo) &&
                    (!textList.Any() || textList.All(t => x.Title.Contains(t)) ||
                     textList.Any(
                         t => t.Length > 2 && x.Posts.Any(p => p.IsPublic && p.Tags.Any(o => o.Name.Contains(t))))) &&
                    (!model.CategoryId.HasValue || catFamilyIds.Contains(x.CategoryId)) &&
                    (string.IsNullOrEmpty(model.Category) || catFamilyIds.Contains(x.CategoryId)) &&
                    (!model.PublishDateFrom.HasValue || x.PublishDate >= model.PublishDateFrom) &&
                    (!model.AuthorId.HasValue || x.OwnerId == model.AuthorId) &&
                    (string.IsNullOrEmpty(model.Author) ||
                     (x.Owner.Profile != null &&
                      (x.Owner.Profile.Firstname + x.Owner.Profile.Lastname).Contains(model.Author)));

            return expression;
        }

        private async Task<List<Guid>> GetCategoryFamilyByName(string categoryName)
        {
            var category = await RepositoryContext.RetrieveAsync<Category>(x => x.Name == categoryName);
            if (category == null)
            {
                return null;
            }

            var allCats = await AllCategories();
            var family = GetBranchItems(category, allCats);
            return family.Select(x => x.Id).ToList();
        }

        private async Task<List<Guid>> GetCategoryFamilyById(Guid categoryId)
        {
            var category = await RepositoryContext.RetrieveAsync<Category>(x => x.Id == categoryId);
            if (category == null)
            {
                return null;
            }

            var allCats = await AllCategories();
            var family = GetBranchItems(category, allCats);
            return family.Select(x => x.Id).ToList();
        }

        private static List<Category> GetBranchItems(Category category, List<Category> categoryList)
        {
            var childs = categoryList.Where(x => x.ParentId == category.Id).ToList();
            var catList = new List<Category>();
            if (childs.Any())
            {
                childs.ForEach(x => catList.AddRange(GetBranchItems(x, categoryList)));
            }
            catList.Add(category);
            return catList;
        }

        private async Task<List<Category>> AllCategories()
        {
            var items = await RepositoryContext.SearchAsync<Category>(x => true);
            if (items == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }
            Result = OperationResult.Success;
            return await items.ToListAsync();
        }

        private async Task<string> GenerateIssueId()
        {
            while (true)
            {
                var topicsCount = await RepositoryContext.CountAsync<Topic>(t => true);
                if (topicsCount == null)
                {
                    return null;
                }

                var digitCount = topicsCount.Value.Length();
                digitCount += 2;
                if (digitCount > 15)
                {
                    digitCount = 15;
                }

                var newId = NumberMethods.RandomFromGuid(digitCount);
                var existedCount = await RepositoryContext.CountAsync<Topic>(t => t.IssueId == newId);
                if (existedCount == 0)
                {
                    return newId;
                }
            }
        }
    }
}
