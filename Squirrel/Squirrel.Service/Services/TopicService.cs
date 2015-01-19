using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squirrel.Data;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;

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


        public async Task AddAsync(Topic topic)
        {
            if (string.IsNullOrEmpty(topic.Title))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var task1 = RepositoryContext.RetrieveAsync<User>(x => x.Id == topic.UserId);
            var task2 = RepositoryContext.RetrieveAsync<Category>(x => x.Id == topic.CategoryId);

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
                FirstPost = topic.FirstPost,
                Title = topic.Title,
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
    }
}
