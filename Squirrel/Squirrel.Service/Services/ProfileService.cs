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
    class ProfileService : IProfileService
    {
        private IRepositoryContext _repositoryContext;
        private IRepositoryContext RepositoryContext
        {
            get { return _repositoryContext ?? (_repositoryContext = DataIOC.Get<IRepositoryContext>()); }
        }

        public OperationResult Result { get; private set; }

        public async Task CreateAsync(Guid userId, Profile profile)
        {
            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            if (user.Profile != null)
            {
                Result = OperationResult.Failed(ServiceMessages.ProfileService_UserHasProfile);
                return;
            }

            var item = new Profile(userId, null)
            {
                AvatarId = profile.AvatarId,
                Firstname = profile.Firstname,
                Lastname = profile.Lastname,
            };

            await RepositoryContext.CreateAsync(item);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public Task UpdateAsync(Profile profile)
        {
            throw new NotImplementedException();
        }

        public Task<Profile> FindByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Profile>> SearchAsync(ProfileSearchModel model)
        {
            throw new NotImplementedException();
        }

        public Task ChangeAvatarAsync(Guid userId, Guid fileId)
        {
            throw new NotImplementedException();
        }
    }
}
