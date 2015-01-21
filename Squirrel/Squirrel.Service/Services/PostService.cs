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

            var attachments = new List<File>();
            if (model.Attachments != null && model.Attachments.Any())
            {
                model
                    .Attachments
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

            var tags = new List<Tag>();
            if (model.Tags != null && model.Tags.Any())
            {
                model
                    .Tags
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

        public Task EditAsync(PostEditModel model, Guid userId)
        {
            throw new NotImplementedException();
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
