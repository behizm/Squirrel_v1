using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.ConfigModels;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace


namespace Squirrel.Service
{
    public interface IUserService
    {
        OperationResult Result { get; }
        UserServiceConfig UserServiceConfig { get; }

        Task CreateAsync(string username, string email, string password);
        Task UpdateAsync(UserUpdateModel user);
        Task<UserDetailsModel> FindByIdAsync(Guid userId);
        Task<UserDetailsModel> FindByUsernameAsync(string username);
        Task<UserDetailsModel> FindByEmailAsync(string username);
        Task<List<UserDetailsModel>> SearchAsync(UserSearchModel model);

        Task ChangePasswordAsync(string oldPassword, string newPassword);
        Task ResetPasswordAsync(Guid userId, string newPassword);
        Task<bool> ValidateAsync(string username, string password);
        Task<bool> LoginAsync(string username, string password);
        Task ActiveAsync(Guid userid);
        Task LockAsync(Guid userId);
        Task UnlockAsync(Guid userId);
    }
}