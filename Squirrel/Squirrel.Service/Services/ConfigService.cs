using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

namespace Squirrel.Service.Services
{
    class ConfigService : BaseService, IConfigService
    {
        public async Task AddAsync(ConfigAddModel model, string username)
        {
            if (string.IsNullOrEmpty(model.Key) || string.IsNullOrEmpty(model.Value))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            model.Key = model.Key.Trim();
            model.Value = model.Value.Trim();
            if (!string.IsNullOrEmpty(model.Description))
            {
                model.Description = model.Description.Trim();
            }

            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Username == username);
            var configTak = RepositoryContext.RetrieveAsync<Config>(x => x.Key.ToLower() == model.Key.ToLower());

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
                Key = model.Key,
                Value = model.Value,
                Description = model.Description
            };
            await RepositoryContext.CreateAsync(item);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task EditAsync(ConfigEditModel model, string username)
        {
            if (string.IsNullOrEmpty(model.Key) || string.IsNullOrEmpty(model.Value))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            model.Key = model.Key.Trim();
            model.Value = model.Value.Trim();
            if (!string.IsNullOrEmpty(model.Description))
            {
                model.Description = model.Description.Trim();
            }

            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Username == username);
            var configTak = RepositoryContext.RetrieveAsync<Config>(x => x.Id == model.Id);

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

            if (config.Value == model.Value && config.Description == model.Description)
            {
                Result = OperationResult.Success;
                return;
            }
            config.Value = model.Value;
            config.Description = model.Description;
            await RepositoryContext.UpdateAsync(config);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task DeleteAsync(Guid id, string username)
        {
            var userTask = RepositoryContext.RetrieveAsync<User>(x => x.Username == username);
            var configTak = RepositoryContext.RetrieveAsync<Config>(x => x.Id == id);

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

        public async Task<string> GetValueAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }
            key = key.Trim();

            var config = await RepositoryContext.RetrieveAsync<Config>(x => x.Key.ToLower() == key.ToLower());
            if (config == null)
            {
                Result = OperationResult.Failed(ServiceMessages.ConfigService_ConfigNotFound);
                return null;
            }

            return config.Value;
        }

        public async Task<Config> FindByIdAsync(Guid id)
        {
            var config = await RepositoryContext.RetrieveAsync<Config>(x => x.Id == id);
            if (config == null)
            {
                Result = OperationResult.Failed(ServiceMessages.ConfigService_ConfigNotFound);
                return null;
            }
            Result = OperationResult.Success;
            return config;
        }

        public async Task<Config> FindByKeyAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }
            key = key.Trim();

            var config = await RepositoryContext.RetrieveAsync<Config>(x => x.Key.ToLower() == key.ToLower());
            if (config == null)
            {
                Result = OperationResult.Failed(ServiceMessages.ConfigService_ConfigNotFound);
                return null;
            }
            Result = OperationResult.Success;
            return config;
        }

        public async Task<List<Config>> SearchAsync(string key, OrderingModel<Config> ordering)
        {
            key = key.Trim();
            var items =
                await RepositoryContext.SearchAsync<Config>(x => string.IsNullOrEmpty(key) || x.Key.Contains(key));

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
                    await
                        items.OrderByDescending(ordering.OrderByKeySelector)
                            .Skip(ordering.Skip)
                            .Take(ordering.Take)
                            .ToListAsync();
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }
        }

        public async Task<int?> CountAsync(string key)
        {
            var count =
                await
                    RepositoryContext.CountAsync<Config>(x => string.IsNullOrEmpty(key) || x.Key.Contains(key));

            if (count == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return count;
        }
    }
}
