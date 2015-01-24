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
    class TagService : ITagService
    {
        public OperationResult Result { get; private set; }

        private IRepositoryContext _repositoryContext;
        private IRepositoryContext RepositoryContext
        {
            get { return _repositoryContext ?? (_repositoryContext = DataIOC.Get<IRepositoryContext>()); }
        }

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
                        await items.OrderBy(ordering.KeySelector).Skip(ordering.Skip).Take(ordering.Take).Select(x => x.Name).ToListAsync();
                }
                return
                        await items.OrderByDescending(ordering.KeySelector).Skip(ordering.Skip).Take(ordering.Take).Select(x => x.Name).ToListAsync();
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
    }
}
