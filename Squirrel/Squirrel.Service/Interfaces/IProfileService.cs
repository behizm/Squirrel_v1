using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface IProfileService : IBaseService
    {
        Task CreateAsync(Guid userId, Profile profile);
        Task CreateAsync(ProfileCreateModel model, string username);
        Task UpdateAsync(Guid userId, Profile profile);
        Task<Profile> FindByIdAsync(Guid userId);
        Task<List<Profile>> SearchAsync(ProfileSearchModel model, OrderingModel<Profile> ordering);
        Task<int?> CountAsync(ProfileSearchModel model);
        Task ChangeAvatarAsync(Guid userId, Guid fileId);
    }
}
