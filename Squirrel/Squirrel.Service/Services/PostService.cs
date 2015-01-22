using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Squirrel.Data;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

namespace Squirrel.Service.Services
{
    class PostService : IPostService
    {
        private IRepositoryContext _repositoryContext;
        private IRepositoryContext RepositoryContext
        {
            get { return _repositoryContext ?? (_repositoryContext = DataIOC.Get<IRepositoryContext>()); }
        }

        public OperationResult Result { get; private set; }


        public async Task AddAsync(PostAddModel model)
        {
            if (model == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            if (string.IsNullOrEmpty(model.Body))
            {
                Result = OperationResult.Failed(ServiceMessages.PostService_EmptyPostBody);
                return;
            }

            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Id == model.UserId);
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
                UserId = user.Id,
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

        public async Task EditAsync(PostEditModel model, Guid userId)
        {
            if (model == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            if (string.IsNullOrEmpty(model.Body))
            {
                Result = OperationResult.Failed(ServiceMessages.PostService_EmptyPostBody);
                return;
            }

            var postTask = RepositoryContext.RetrieveAsync<Post>(x => x.Id == model.Id);
            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
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
            if (!user.IsAdmin && post.UserId != user.Id)
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
            post.UserId = user.Id;

            await UpdateAsync(post);
        }

        public Task DeleteAsync(PostRemoveModel model, Guid userId)
        {
            throw new NotImplementedException();
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

        public Task<List<Post>> SearchAsync(PostSearchModel model, OrderingModel<Post> ordering)
        {
            throw new NotImplementedException();
        }

        public Task PublicPostAsync(Guid id, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task PrivatePostAsync(Guid id, Guid userId)
        {
            throw new NotImplementedException();
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
    }
}
