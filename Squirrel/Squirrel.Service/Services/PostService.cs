using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

namespace Squirrel.Service.Services
{
    class PostService : BaseService, IPostService
    {
        public async Task AddAsync(PostAddSimpleModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }
            model.Username = model.Username.ToLower();

            if (string.IsNullOrEmpty(model.Body))
            {
                Result = OperationResult.Failed(ServiceMessages.PostService_EmptyPostBody);
                return;
            }

            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Username.ToLower() == model.Username);
            var topicTask = RepositoryContext.RetrieveAsync<Topic>(x => x.Id == model.TopicId);

            var user = await userTask;
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            var topic = await topicTask;
            if (topic == null)
            {
                Result = OperationResult.Failed(ServiceMessages.TopicService_TopicNotFound);
                return;
            }

            var post = new Post
            {
                Body = model.Body,
                IsPublic = false,
                TopicId = topic.Id,
                AuthorId = user.Id,
            };
            await RepositoryContext.CreateAsync(post);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task AddAsync(PostAddModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }
            model.Username = model.Username.ToLower();

            if (string.IsNullOrEmpty(model.Body))
            {
                Result = OperationResult.Failed(ServiceMessages.PostService_EmptyPostBody);
                return;
            }

            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Username.ToLower() == model.Username);
            var topicTask = RepositoryContext.RetrieveAsync<Topic>(x => x.Id == model.TopicId);

            var user = await userTask;
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            var topic = await topicTask;
            if (topic == null)
            {
                Result = OperationResult.Failed(ServiceMessages.TopicService_TopicNotFound);
                return;
            }

            var attachmentsTask = GetAttachmentsAsync(model.Attachments);
            var tagsTask = GetTagsAsync(model.Tags);

            var attachments = await attachmentsTask;
            var tags = await tagsTask;

            var post = new Post
            {
                Attachments = attachments,
                Abstract = model.Abstract,
                Body = model.Body,
                HeaderImageId = model.HeaderImageId,
                IsPublic = false,
                TopicId = topic.Id,
                AuthorId = user.Id,
                Tags = tags,
            };
            await RepositoryContext.CreateAsync(post);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task EditAsync(PostEditModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }
            model.Username = model.Username.ToLower();

            if (string.IsNullOrEmpty(model.Body))
            {
                Result = OperationResult.Failed(ServiceMessages.PostService_EmptyPostBody);
                return;
            }

            var postTask = RepositoryContext.RetrieveAsync<Post>(x => x.Id == model.Id);
            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Username.ToLower() == model.Username);
            var topicTask = RepositoryContext.RetrieveAsync<Topic>(x => x.Id == model.TopicId);

            var post = await postTask;
            if (post == null)
            {
                Result = OperationResult.Failed(ServiceMessages.PostService_PostNotFound);
                return;
            }

            var user = await userTask;
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            if (!user.IsAdmin && post.AuthorId != user.Id)
            {
                Result = OperationResult.Failed(ServiceMessages.PostService_NoAccess);
                return;
            }

            var topic = await topicTask;
            if (topic == null)
            {
                Result = OperationResult.Failed(ServiceMessages.TopicService_TopicNotFound);
                return;
            }

            model.Tags = model.Tags ?? new List<string>();
            var newTag = model.Tags.Where(x => post.Tags.All(t => t.Name.Trim().ToLower() != x.Trim().ToLower())).ToList();
            var removeTag = new List<Tag>();
            post.Tags.ForEach(t =>
            {
                if (model.Tags.All(x => x.Trim().ToLower() != t.Name.Trim().ToLower()))
                {
                    removeTag.Add(t);
                }
            });
            removeTag.ForEach(x => post.Tags.Remove(x));

            model.Attachments = model.Attachments ?? new List<Guid>();
            var newAttach = model.Attachments.Where(x => post.Attachments.All(a => a.Id != x)).ToList();
            var removeAttach = new List<File>();
            post.Attachments.ForEach(a =>
            {
                if (!model.Attachments.Contains(a.Id))
                {
                    removeAttach.Add(a);
                }
            });
            removeAttach.ForEach(x => post.Attachments.Remove(x));

            var tagsTask = GetTagsAsync(newTag);
            var attachmentsTask = GetAttachmentsAsync(newAttach);

            var tags = await tagsTask;
            var attachments = await attachmentsTask;

            tags.ForEach(x => post.Tags.Add(x));
            attachments.ForEach(x => post.Attachments.Add(x));
            post.Abstract = model.Abstract;
            post.Body = model.Body;
            post.HeaderImageId = model.HeaderImageId;
            post.TopicId = topic.Id;
            post.AuthorId = user.Id;
            post.IsPublic = model.IsPublic;

            await UpdateAsync(post);
        }

        public async Task DeleteAsync(PostRemoveModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }
            model.Username = model.Username.ToLower();

            var postTask = WarehouseContext.RetrieveAsync<Post>(x => x.Id == model.Id);
            var userTask = WarehouseContext.RetrieveAsync<User>(x => x.Username == model.Username);

            var post = await postTask;
            if (post == null)
            {
                Result = OperationResult.Failed(ServiceMessages.PostService_PostNotFound);
                return;
            }

            var user = await userTask;
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            if (!user.IsAdmin && user.Id != post.AuthorId)
            {
                Result = OperationResult.Failed(ServiceMessages.PostService_NoAccess);
                return;
            }

            if (post.Comments != null && post.Comments.Any())
                WarehouseContext.Delete(post.Comments);

            if (post.Votes != null && post.Votes.Any())
                WarehouseContext.Delete(post.Votes);

            if (post.IsPublic && !post.Topic.Posts.Any(p => p.Id != post.Id && p.IsPublic))
            {
                post.Topic.IsPublished = false;
                WarehouseContext.Update(post.Topic);
            }
            WarehouseContext.Delete(post);
            await WarehouseContext.SaveChangesAsync();
            if (WarehouseContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task<Post> FindByIdAsync(Guid id)
        {
            var post = await RepositoryContext.RetrieveAsync<Post>(x => x.Id == id);
            if (post == null)
            {
                Result = OperationResult.Failed(ServiceMessages.PostService_PostNotFound);
                return null;
            }
            Result = OperationResult.Success;
            return post;
        }

        public async Task<List<Post>> SearchAsync(PostSearchModel model, OrderingModel<Post> ordering)
        {
            if (model == null || ordering == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var items =
                await
                    RepositoryContext.SearchAsync<Post>(p =>
                        (string.IsNullOrEmpty(model.Abstract) || p.Abstract.Contains(model.Abstract)) &&
                        (string.IsNullOrEmpty(model.Body) || p.Body.Contains(model.Body)) &&
                        (string.IsNullOrEmpty(model.Topic) || p.Topic.Title.Contains(model.Topic)) &&
                        (string.IsNullOrEmpty(model.User) || p.Author.Username == model.User) &&
                        (!model.IsPublic.HasValue || p.IsPublic == model.IsPublic));

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
                        await items.OrderBy(ordering.OrderByKeySelector).Skip(ordering.Skip).Take(ordering.Take).ToListAsync();
                }
                return
                        await items.OrderByDescending(ordering.OrderByKeySelector).Skip(ordering.Skip).Take(ordering.Take).ToListAsync();
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }
        }

        public async Task<int?> CountAsync(PostSearchModel model)
        {
            if (model == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var count =
                await
                    RepositoryContext.CountAsync<Post>(p =>
                        (string.IsNullOrEmpty(model.Abstract) || p.Abstract.Contains(model.Abstract)) &&
                        (string.IsNullOrEmpty(model.Body) || p.Body.Contains(model.Body)) &&
                        (string.IsNullOrEmpty(model.Topic) || p.Topic.Title.Contains(model.Topic)) &&
                        (string.IsNullOrEmpty(model.User) || p.Author.Username == model.User) &&
                        (!model.IsPublic.HasValue || p.IsPublic == model.IsPublic));

            if (count == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return count;
        }

        public async Task PublicPostAsync(Guid id, Guid userId)
        {
            await ChangePublic(id, userId, true);
        }

        public async Task PrivatePostAsync(Guid id, Guid userId)
        {
            await ChangePublic(id, userId, false);
        }



        private async Task<List<File>> GetAttachmentsAsync(List<Guid> items)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    var attachments = new List<File>();
                    if (items != null && items.Any())
                    {
                        items
                            .Select(a => RepositoryContext.RetrieveAsync<File>(x => x.Id == a))
                            .ToList()
                            .ForEach(async a =>
                            {
                                var file = await a;
                                if (file == null)
                                {
                                    return;
                                }
                                attachments.Add(file);
                            });
                    }
                    return attachments;
                }
                catch (Exception)
                {
                    return new List<File>();
                }
            });

            return await task;
        }

        private async Task<List<Tag>> GetTagsAsync(List<string> items)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    var tags = new List<Tag>();
                    if (items != null && items.Any())
                    {
                        items
                            .Select(t => t.Trim())
                            .Select(t => new TaskDictionary<Tag>
                            {
                                Name = t,
                                Task = RepositoryContext.RetrieveAsync<Tag>(x => x.Name.ToLower() == t.ToLower())
                            })
                            .ToList()
                            .ForEach(async a =>
                            {
                                var tag = await a.Task;
                                tags.Add(tag ?? new Tag { Name = a.Name });
                            });
                    }
                    return tags;
                }
                catch (Exception)
                {
                    return new List<Tag>();
                }
            });
            return await task;
        }

        private async Task UpdateAsync(Post post)
        {
            post.EditDate = DateTime.Now;
            await RepositoryContext.UpdateAsync(post);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        private async Task ChangePublic(Guid postId, Guid userId, bool isPublic)
        {
            var postTask = RepositoryContext.RetrieveAsync<Post>(x => x.Id == postId);
            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);

            var post = await postTask;
            if (post == null)
            {
                Result = OperationResult.Failed(ServiceMessages.PostService_PostNotFound);
                return;
            }

            var user = await userTask;
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            if (!user.IsAdmin && user.Id != post.AuthorId)
            {
                Result = OperationResult.Failed(ServiceMessages.PostService_NoAccess);
                return;
            }

            if (post.IsPublic == isPublic)
            {
                Result = OperationResult.Success;
                return;
            }

            post.IsPublic = isPublic;
            if (isPublic && !post.PublishDate.HasValue)
            {
                post.PublishDate = DateTime.Now;
            }
            await UpdateAsync(post);
        }
    }
}
