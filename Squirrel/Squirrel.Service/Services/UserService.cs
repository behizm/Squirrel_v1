using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Squirrel.Data;
using Squirrel.Domain.ConfigModels;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;
using Squirrel.Utility.Cryptography;
using Squirrel.Utility.Helpers;

namespace Squirrel.Service.Services
{
    class UserService : BaseService, IUserService
    {
        public UserServiceConfig UserServiceConfig { get; private set; }

        public UserService()
        {
            UserServiceConfig = new UserServiceConfig
            {
                AutoActive = false,
                LockTimeSpan = TimeSpan.FromMinutes(30),
                MaximumAccessFailed = 5,
                MinimumPasswordLenght = 6,
            };
        }


        public async Task CreateAsync(string username, string email, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            if (password.Length < UserServiceConfig.MinimumPasswordLenght)
            {
                Result =
                    OperationResult.Failed(
                        string.Format(ServiceMessages.UserService_CreateAsync_PasswordLenght,
                            UserServiceConfig.MinimumPasswordLenght));
                return;
            }

            var oldUsers = await RepositoryContext.SearchAsync<User>(x => x.Username == username);
            if (oldUsers == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }
            if (oldUsers.Any())
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_CreateAsync_UsernameDuplicate);
                return;
            }

            oldUsers = await RepositoryContext.SearchAsync<User>(x => x.Email == email);
            if (oldUsers == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }
            if (oldUsers.Any())
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_CreateAsync_EmailDuplicate);
                return;
            }

            var passwordHash = await SquirrelHashSystem.EncryptAsync(password);
            if (string.IsNullOrEmpty(passwordHash))
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }

            var user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                Username = username,
            };
            await CreateAsync(user);
        }

        public async Task UpdateAsync(Guid id, string username, string email)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == id);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            var oldUsers = await RepositoryContext.SearchAsync<User>(x => x.Username == username && x.Id != user.Id);
            if (oldUsers == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }
            if (oldUsers.Any())
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_CreateAsync_UsernameDuplicate);
                return;
            }

            oldUsers = await RepositoryContext.SearchAsync<User>(x => x.Email == email && x.Id != user.Id);
            if (oldUsers == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }
            if (oldUsers.Any())
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_CreateAsync_EmailDuplicate);
                return;
            }

            user.Email = email;
            user.Username = username;
            await UpdateAsync(user);
        }

        public async Task UpdateAsync(UserUpdateModel model)
        {
            if (model.Username.IsNothing() || model.Email.IsNothing())
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == model.Id);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            user.Email = model.Email;
            user.Username = model.Username;
            user.IsActive = model.IsActive;
            user.IsLock = model.IsLock;
            user.IsAdmin = model.IsAdmin;
            await UpdateAsync(user);
        }

        public async Task RemoveAsync(Guid id)
        {
            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == id);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            await RepositoryContext.DeleteAsync(user);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task<User> FindByIdAsync(Guid userId)
        {
            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.FileService_FileNotFount);
                return null;
            }

            Result = OperationResult.Success;
            return user;
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Username == username);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.FileService_FileNotFount);
                return null;
            }

            Result = OperationResult.Success;
            return user;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Email == email);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.FileService_FileNotFount);
                return null;
            }

            Result = OperationResult.Success;
            return user;
        }

        public async Task<List<User>> SearchAsync(UserSearchModel model, OrderingModel<User> ordering)
        {
            var items =
                await RepositoryContext.SearchAsync<User>(x =>
                    (!model.Id.HasValue || model.Id.Value == x.Id) &&
                    (string.IsNullOrEmpty(model.Username) || x.Username.Contains(model.Username)) &&
                    (string.IsNullOrEmpty(model.Email) || x.Email.Contains(model.Email)) &&
                    (!model.IsActive.HasValue || model.IsActive.Value == x.IsActive) &&
                    (!model.CreateDateFrom.HasValue || model.CreateDateFrom.Value <= x.CreateDate) &&
                    (!model.CreateDateTo.HasValue || model.CreateDateTo.Value >= x.CreateDate) &&
                    (!model.LastLoginFrom.HasValue || model.LastLoginFrom.Value <= x.LastLogin) &&
                    (!model.LastLoginTo.HasValue || model.LastLoginTo.Value >= x.LastLogin));

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
                        await items.OrderBy(ordering.OrderByKeySelector).Skip(ordering.Skip).Take(ordering.Take).ToListAsync();
                }
                return
                        await items.OrderByDescending(ordering.OrderByKeySelector).Skip(ordering.Skip).Take(ordering.Take).ToListAsync();
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }
        }

        public async Task<int?> CountAsync(UserSearchModel model)
        {
            var count =
                await RepositoryContext.CountAsync<User>(x =>
                    (!model.Id.HasValue || model.Id.Value == x.Id) &&
                    (string.IsNullOrEmpty(model.Username) || x.Username.Contains(model.Username)) &&
                    (string.IsNullOrEmpty(model.Email) || x.Email.Contains(model.Email)) &&
                    (!model.IsActive.HasValue || model.IsActive.Value == x.IsActive) &&
                    (!model.CreateDateFrom.HasValue || model.CreateDateFrom.Value <= x.CreateDate) &&
                    (!model.CreateDateTo.HasValue || model.CreateDateTo.Value >= x.CreateDate) &&
                    (!model.LastLoginFrom.HasValue || model.LastLoginFrom.Value <= x.LastLogin) &&
                    (!model.LastLoginTo.HasValue || model.LastLoginTo.Value >= x.LastLogin));

            if (count == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return count;
        }

        public async Task ChangePasswordAsync(string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            if (oldPassword == newPassword)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_ChangePasswordAsync_SamePassword);
                return;
            }

            if (newPassword.Length < UserServiceConfig.MinimumPasswordLenght)
            {
                Result =
                    OperationResult.Failed(
                        string.Format(ServiceMessages.UserService_CreateAsync_PasswordLenght,
                            UserServiceConfig.MinimumPasswordLenght));
                return;
            }

            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Username == username);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            var password = await SquirrelHashSystem.DecryptAsync(user.PasswordHash);
            if (password == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }

            if (password != oldPassword)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_WrongPassword);
                return;
            }

            var passwordHash = await SquirrelHashSystem.EncryptAsync(newPassword);
            if (passwordHash == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }
            user.PasswordHash = passwordHash;
            await UpdateAsync(user);
        }

        public async Task ResetPasswordAsync(Guid userId, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            if (newPassword.Length < UserServiceConfig.MinimumPasswordLenght)
            {
                Result =
                    OperationResult.Failed(
                        string.Format(ServiceMessages.UserService_CreateAsync_PasswordLenght,
                            UserServiceConfig.MinimumPasswordLenght));
                return;
            }

            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            var passwordHash = await SquirrelHashSystem.EncryptAsync(newPassword);
            if (passwordHash == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }
            user.PasswordHash = passwordHash;
            await UpdateAsync(user);
        }

        public async Task<bool> ValidateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return false;
            }

            var passwordHash = await SquirrelHashSystem.EncryptAsync(password);
            if (passwordHash == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return false;
            }

            var user =
                await
                    RepositoryContext.RetrieveAsync<User>(
                        x => (x.Username == username || x.Email == username) && x.PasswordHash == passwordHash);

            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return false;
            }

            Result = OperationResult.Success;
            return true;
        }

        public async Task<string> LoginAsync(string username, string email, string password)
        {
            if ((string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email)) || string.IsNullOrEmpty(password))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Username == username || x.Email == email);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_LoginAsync_Wrong);
                return null;
            }

            if (!user.IsActive)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_LoginAsync_IsActive);
                return null;
            }

            if (user.LockDate.HasValue && user.LockDate.Value > DateTime.Now)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_LoginAsync_LockDate);
                return null;
            }

            if (user.IsLock)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_LoginAsync_IsLock);
                return null;
            }

            var passwordHash = await SquirrelHashSystem.EncryptAsync(password);
            if (passwordHash == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            if (passwordHash != user.PasswordHash)
            {
                await AccessFailedAsync(user);
                Result = OperationResult.Failed(ServiceMessages.UserService_LoginAsync_Wrong);
                return null;
            }

            user.LastLogin = DateTime.Now;
            user.AccessFailed = 0;
            await UpdateAsync(user);
            Result = OperationResult.Success;
            return user.Username;
        }

        public async Task ActiveAsync(Guid userId)
        {
            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            user.IsActive = true;
            await UpdateAsync(user);
        }

        public async Task LockAsync(Guid userId)
        {
            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            if (user.IsLock)
            {
                Result = OperationResult.Success;
                return;
            }

            user.IsLock = true;
            await UpdateAsync(user);
        }

        public async Task UnlockAsync(Guid userId)
        {
            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            if ((!user.LockDate.HasValue || user.LockDate.Value < DateTime.Now) && !user.IsLock)
            {
                Result = OperationResult.Success;
                return;
            }

            if (user.LockDate.HasValue && user.LockDate.Value > DateTime.Now)
            {
                user.LockDate = DateTime.Now;
            }

            if (user.IsLock)
            {
                user.IsLock = false;
            }

            user.LockDate = DateTime.Now;
            await UpdateAsync(user);
        }

        public async Task ChangeAdminAsync(Guid userId, bool isAdmin)
        {
            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            user.IsAdmin = isAdmin;
            user.LockDate = DateTime.Now;
            await UpdateAsync(user);
        }

        public async Task<ListModel<Topic>> PublishedTopicsAsync(Guid userId, int skip, int take)
        {
            var items = await
                RepositoryContext.SearchAsync<Topic>(x =>
                    x.IsPublished &&
                    x.PublishDate <= DateTime.Now &&
                    (x.OwnerId == userId || (x.Posts.Any(p => p.IsPublic && p.AuthorId == userId))));
            if (items == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return new ListModel<Topic>
            {
                CountOfAll = items.Count(),
                List = items.OrderByDescending(x => x.PublishDate).Skip(skip).Take(take).ToList(),
            };
        }


        private async Task CreateAsync(User user)
        {
            user.AccessFailed = 0;
            user.IsActive = UserServiceConfig.AutoActive;
            user.IsLock = false;

            await RepositoryContext.CreateAsync(user);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        private async Task UpdateAsync(User user)
        {
            user.EditeDate = DateTime.Now;
            await RepositoryContext.UpdateAsync(user);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        private async Task AccessFailedAsync(User user)
        {
            user.AccessFailed++;

            if (user.AccessFailed >= UserServiceConfig.MaximumAccessFailed)
                user.LockDate = DateTime.Now.Add(UserServiceConfig.LockTimeSpan);

            await UpdateAsync(user);
        }
    }
}
