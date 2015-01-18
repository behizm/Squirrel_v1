using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface IProfileService
    {
        OperationResult Result { get; }

        Task CreateAsync(Guid userId, Profile profile);
        Task UpdateAsync(Profile profile);
        Task<Profile> FindByIdAsync(Guid userId);
        Task<List<Profile>> SearchAsync(ProfileSearchModel model);
        Task ChangeAvatarAsync(Guid userId, Guid fileId);
    }
}
