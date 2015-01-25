using System;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;

namespace Squirrel.Service.Services
{
    class ConfigService : BaseService, IConfigService
    {
        public async Task AddAsync(string key, string value, Guid userId)
        {
            key = key.Trim();
            value = value.Trim();

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            var configTak = RepositoryContext.RetrieveAsync<Config>(x => x.Key.ToLower() == key.ToLower());

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

            var config = await configTak;
            if (config != null)
            {
                Result = OperationResult.Failed(ServiceMessages.ConfigService_ConfigExisted);
                return;
            }

            var item = new Config
            {
                Key = key,
                Value = value,
            };
            await RepositoryContext.CreateAsync(item);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task EditAsync(string key, string value, Guid userId)
        {
            key = key.Trim();
            value = value.Trim();

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            var configTak = RepositoryContext.RetrieveAsync<Config>(x => x.Key.ToLower() == key.ToLower());

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

            var config = await configTak;
            if (config == null)
            {
                Result = OperationResult.Failed(ServiceMessages.ConfigService_ConfigNotFound);
                return;
            }

            if (config.Value == value)
            {
                Result = OperationResult.Success;
                return;
            }
            config.Value = value;
            await RepositoryContext.UpdateAsync(config);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task DeleteAsync(string key, Guid userId)
        {
            key = key.Trim();

            if (string.IsNullOrEmpty(key))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            var configTak = RepositoryContext.RetrieveAsync<Config>(x => x.Key.ToLower() == key.ToLower());

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

            var config = await configTak;
            if (config == null)
            {
                Result = OperationResult.Failed(ServiceMessages.ConfigService_ConfigNotFound);
                return;
            }

            await RepositoryContext.DeleteAsync(config);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task<string> GetAsync(string key)
        {
            key = key.Trim();

            if (string.IsNullOrEmpty(key))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var config = await RepositoryContext.RetrieveAsync<Config>(x => x.Key.ToLower() == key.ToLower());
            if (config == null)
            {
                Result = OperationResult.Failed(ServiceMessages.ConfigService_ConfigNotFound);
                return null;
            }

            return config.Value;
        }
    }
}
