using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;
using Squirrel.Utility.Helpers;

namespace Squirrel.Service.Services
{
    class TagService : BaseService, ITagService
    {
        public async Task<List<string>> SearchAsync(TagSearchModel model, OrderingModel<Tag> ordering)
        {
            var items =
                await
                    RepositoryContext.SearchAsync<Tag>(
                        x => string.IsNullOrEmpty(model.Name) || x.Name.Contains(model.Name));

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
                        await items.OrderBy(ordering.OrderByKeySelector).Skip(ordering.Skip).Take(ordering.Take).Select(x => x.Name).ToListAsync();
                }
                return
                        await items.OrderByDescending(ordering.OrderByKeySelector).Skip(ordering.Skip).Take(ordering.Take).Select(x => x.Name).ToListAsync();
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }
        }

        public async Task<int?> CountAsync(TagSearchModel model)
        {
            var count =
                await
                    RepositoryContext.CountAsync<Tag>(
                        x => string.IsNullOrEmpty(model.Name) || x.Name.Contains(model.Name));

            if (count == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return count;
        }

        public async Task DeleteAsync(Guid tagId, Guid userId)
        {
            var tagTask = RepositoryContext.RetrieveAsync<Tag>(x => x.Id == tagId);
            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);

            var tag = await tagTask;
            if (tag == null)
            {
                Result = OperationResult.Failed(ServiceMessages.TagService_TagNotFound);
                return;
            }

            var user = await userTask;
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            if (!user.IsAdmin)
            {
                Result = OperationResult.Failed(ServiceMessages.General_NoAccessForThisOp);
                return;
            }

            await RepositoryContext.DeleteAsync(tag);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task<List<Topic>> PublishedTopicsAsync(string tagName, int skip, int take)
        {
            if (tagName.IsNothing())
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }
            tagName = tagName.TrimAndLower();

            var tag = await RepositoryContext.RetrieveAsync<Tag>(x => x.Name.ToLower() == tagName);
            if (tag == null)
            {
                Result = OperationResult.Failed(ServiceMessages.TagService_TagNotFound);
                return null;
            }

            var topics =
                await RepositoryContext.SearchAsync<Topic>(x => x.Posts.Any(p => p.Tags.Any(t => t.Id == tag.Id)));
            if (topics == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return
                await topics
                    .Where(x => x.IsPublished && x.PublishDate.HasValue && x.PublishDate <= DateTime.Now)
                    .OrderByDescending(x => x.PublishDate)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
        }

        public async Task<ListModel<TagWeightModel>> TagsWithWeightAsync<TKey>(OrderingModel<TagWeightModel, TKey> ordering, bool isPublished = false)
        {
            var posts =
                await
                    RepositoryContext.SearchAsync<Post>(
                        x => !isPublished || (x.IsPublic && x.Topic.IsPublished && x.Topic.PublishDate <= DateTime.Now));
            if (posts == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            var tags = await posts.SelectMany(x => x.Tags).Select(x => x.Name).ToListAsync();
            var uniqueTags = tags.Distinct().ToList();

            var resultModel = new ListModel<TagWeightModel>
            {
                CountOfAll = uniqueTags.Count(),
                List =
                    uniqueTags.Select(x => new TagWeightModel { Name = x, Weight = tags.Count(t => t == x) }).ToList()
            };

            try
            {
                Result = OperationResult.Success;
                if (ordering.IsAscending)
                {
                    resultModel.List =
                        resultModel.List
                            .OrderBy(ordering.OrderByKeySelectorFunc)
                            .Skip(ordering.Skip)
                            .Take(ordering.Take)
                            .ToList();
                }
                else
                {
                    resultModel.List =
                        resultModel.List
                            .OrderByDescending(ordering.OrderByKeySelectorFunc)
                            .Skip(ordering.Skip)
                            .Take(ordering.Take)
                            .ToList();
                }
                return resultModel;
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }
        }

    }
}
