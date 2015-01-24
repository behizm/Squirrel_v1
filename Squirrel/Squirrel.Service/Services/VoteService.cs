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
    class VoteService : IVoteService
    {
        private IRepositoryContext _repositoryContext;
        private IRepositoryContext RepositoryContext
        {
            get { return _repositoryContext ?? (_repositoryContext = DataIOC.Get<IRepositoryContext>()); }
        }

        private IWarehouseContext _warehouseContext;
        private IWarehouseContext WarehouseContext
        {
            get { return _warehouseContext ?? (_warehouseContext = DataIOC.Get<IWarehouseContext>()); }
        }

        public OperationResult Result { get; private set; }


        public async Task LikeAsync(Guid postId, Guid userId)
        {
            await ChangePlus(postId, userId, true);
        }

        public async Task DisLikeAsync(Guid postId, Guid userId)
        {
            await ChangePlus(postId, userId, false);
        }

        public async Task<bool?> FindAsync(Guid postId, Guid userId)
        {
            var vote = await RepositoryContext.RetrieveAsync<Vote>(x => x.UserId == userId && x.PostId == postId);
            if (vote != null)
                return vote.Plus;
            Result = OperationResult.Failed(ServiceMessages.VoteService_VoteNotFound);
            return null;
        }


        private async Task ChangePlus(Guid postId, Guid userId, bool plus)
        {
            var vote = await WarehouseContext.RetrieveAsync<Vote>(x => x.PostId == postId && x.UserId == userId);
            if (vote != null)
            {
                if (vote.Plus == plus)
                {
                    Result = OperationResult.Success;
                    return;
                }
                WarehouseContext.Delete(vote);
            }
            var item = new Vote
            {
                Plus = plus,
                PostId = postId,
                UserId = userId,
            };
            WarehouseContext.Create(item);
            await WarehouseContext.SaveChangesAsync();
            if (WarehouseContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }
    }
}
