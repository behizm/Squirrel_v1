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

        public async Task UpdateAsync(Guid userId, Profile profile)
        {
            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            if (user.Profile == null)
            {
                Result = OperationResult.Failed(ServiceMessages.ProfileService_UserHasNotProfile);
                return;
            }

            user.Profile.AvatarId = profile.AvatarId;
            user.Profile.EditDate = DateTime.Now;
            user.Profile.Firstname = profile.Firstname;
            user.Profile.Lastname = profile.Lastname;

            await RepositoryContext.UpdateAsync(user.Profile);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task<Profile> FindByIdAsync(Guid userId)
        {
            var item = await RepositoryContext.RetrieveAsync<Profile>(x => x.UserId == userId);
            if (item == null)
            {
                Result = OperationResult.Failed(ServiceMessages.ProfileService_ProfileNotFound);
                return null;
            }

            Result = OperationResult.Success;
            return item;
        }

        public async Task<List<Profile>> SearchAsync(ProfileSearchModel model, int skip, int take)
        {
            var items =
                await RepositoryContext.SearchAsync<Profile>(x =>
                    (string.IsNullOrEmpty(model.FirstName) || x.Firstname.Contains(model.FirstName)) &&
                    (string.IsNullOrEmpty(model.LastName) || x.Lastname.Contains(model.LastName)) &&
                    (string.IsNullOrEmpty(model.Username) || x.User.Username.Contains(model.Username)) &&
                    (!model.UserId.HasValue || x.UserId == model.UserId));

            if (items == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return await items.OrderBy(x => x.Lastname).Skip(skip).Take(take).ToListAsync();
        }

        public async Task ChangeAvatarAsync(Guid userId, Guid fileId)
        {
            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            if (user.Profile == null)
            {
                Result = OperationResult.Failed(ServiceMessages.ProfileService_UserHasNotProfile);
                return;
            }

            user.Profile.AvatarId = fileId;

            await RepositoryContext.UpdateAsync(user.Profile);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

    }
}
