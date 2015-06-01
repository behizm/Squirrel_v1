using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.ConfigModels;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface IUserService : IBaseService
    {
        UserServiceConfig UserServiceConfig { get; }

        Task CreateAsync(string username, string email, string password);
        Task UpdateAsync(Guid id, string username, string email);
        Task UpdateAsync(UserUpdateModel model);
        Task RemoveAsync(Guid id);
        Task<User> FindByIdAsync(Guid userId);
        Task<User> FindByUsernameAsync(string username);
        Task<User> FindByEmailAsync(string email);
        Task<List<User>> SearchAsync(UserSearchModel model, OrderingModel<User> ordering);
        Task<int?> CountAsync(UserSearchModel model);

        Task ChangePasswordAsync(string username, string oldPassword, string newPassword);
        Task ResetPasswordAsync(Guid userId, string newPassword);
        Task<bool> ValidateAsync(string username, string password);
        Task<string> LoginAsync(string username, string email, string password);
        Task ActiveAsync(Guid userId);
        Task LockAsync(Guid userId);
        Task UnlockAsync(Guid userId);
        Task ChangeAdminAsync(Guid userId, bool isAdmin);

        Task<ListModel<Topic>> PublishedTopicsAsync(Guid userId, int skip, int take);
    }
}