using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Squirrel.Data;
using Squirrel.Domain.ConfigModels;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

namespace Squirrel.Service.Services
{
    class UserService : IUserService
    {
        public OperationResult Result { get; private set; }

        public UserServiceConfig UserServiceConfig { get; private set; }

        private IRepositoryContext _repositoryContext;
        private IRepositoryContext RepositoryContext
        {
            get { return _repositoryContext ?? (_repositoryContext = DataIOC.Get<IRepositoryContext>()); }
        }


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

            var passwordHash = await Hashing(password);
            if (string.IsNullOrEmpty(passwordHash))
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }

            var user = new User
            {
                AccessFailed = 0,
                Email = email,
                IsActive = UserServiceConfig.AutoActive,
                IsLock = false,
                PasswordHash = passwordHash,
                Username = username,
            };
            await RepositoryContext.CreateAsync(user);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public Task UpdateAsync(UserUpdateModel user)
        {
            throw new NotImplementedException();
        }

        public Task<UserDetailsModel> FindByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDetailsModel> FindByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<UserDetailsModel> FindByEmailAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDetailsModel>> SearchAsync(UserSearchModel model)
        {
            throw new NotImplementedException();
        }

        public Task ChangePasswordAsync(string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task ResetPasswordAsync(Guid userId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task ActiveAsync(Guid userid)
        {
            throw new NotImplementedException();
        }

        public Task LockAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UnlockAsync(Guid userId)
        {
            throw new NotImplementedException();
        }


        private static async Task<string> Hashing(string code)
        {
            return 
                await Domain.Cryptography.Symmetric<TripleDESCryptoServiceProvider>
                    .EncryptAsync(code, "behnamZeighami", "snjab");

        }
    }
}
