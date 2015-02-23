using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;
using Squirrel.Utility.Helpers;

namespace Squirrel.Service.Services
{
    class LogService : BaseService, ILogService
    {
        public async Task AddAsync(LogAddModel model)
        {
            if (model == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            User user = null;
            if (!model.Username.IsEmpty())
            {
                model.Username = model.Username.TrimAndLower();
                user = await RepositoryContext.RetrieveAsync<User>(x => x.Username == model.Username);
            }

            var log = new Log
            {
                Action = model.Action,
                Area = model.Area,
                Controller = model.Controller,
                ReferredHost = model.ReferredHost,
                IsAjax = model.IsAjax
            };

            if (user != null)
            {
                log.UserId = user.Id;
            }

            var logInfo = new LogInfo
            {
                FullUrl = model.FullUrl,
                Ip = model.Ip,
                Port = model.Port,
                ReferredUrl = model.ReferredUrl,
                UserAgent = model.UserAgent,
            };
            log.InfoId = logInfo.Id;
            log.Info = logInfo;

            if (!model.ErrorMessage.IsEmpty())
            {
                var error = new Error
                {
                    IsPostMethod = model.IsPostMethod,
                    Message = model.ErrorMessage,
                    LineNumber = model.LineNumber,
                };
                log.ErrorId = error.Id;
                log.Error = error;
            }

            await RepositoryContext.CreateAsync(log);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(RepositoryContext.OperationResult.Errors);
        }

        public async Task DeleteAsync(LogRemoveModel model)
        {
            if (model == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var log = await RepositoryContext.RetrieveAsync<Log>(x => x.Id == model.Id);
            if (log == null)
            {
                Result = OperationResult.Failed(ServiceMessages.LogService_LogNotFound);
                return;
            }

            await RepositoryContext.DeleteAsync(log);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(RepositoryContext.OperationResult.Errors);
        }

        public async Task<Log> FindByIdAsync(Guid id)
        {
            var log = await RepositoryContext.RetrieveAsync<Log>(x => x.Id == id);
            if (log != null)
            {
                Result = OperationResult.Success;
                return log;
            }

            Result = OperationResult.Failed(RepositoryContext.OperationResult.Errors);
            return null;
        }

        public async Task<List<Log>> SearchAsync<TKey>(LogSearchModel model, OrderingModel<Log, TKey> ordering)
        {
            if (model == null || ordering == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }
            model.Username = model.Username.TrimAndLower();

            var items =
                await
                    RepositoryContext.SearchAsync<Log>(x =>
                        (model.Action.IsEmpty() || x.Action.Contains(model.Action)) &&
                        (model.Area.IsEmpty() || x.Area.Contains(model.Area)) &&
                        (model.Controller.IsEmpty() || x.Controller.Contains(model.Controller)) &&
                        (model.ReferredHost.IsEmpty() || x.ReferredHost.Contains(model.ReferredHost)) &&
                        (!model.IsAjax.HasValue || x.IsAjax == model.IsAjax) &&
                        (model.FullUrl.IsEmpty() || (x.InfoId != null && x.Info.FullUrl.Contains(model.FullUrl))) &&
                        (model.ReferredUrl.IsEmpty() || (x.InfoId != null && x.Info.ReferredUrl.Contains(model.ReferredUrl))) &&
                        (model.UserAgent.IsEmpty() || (x.InfoId != null && x.Info.UserAgent.Contains(model.UserAgent))) &&
                        (model.Ip.IsEmpty() || (x.InfoId != null && x.Info.Ip.Contains(model.Ip))) &&
                        (model.Port.IsEmpty() || (x.InfoId != null && x.Info.Port.Contains(model.Port))) &&
                        (!model.IsPostMethod.HasValue || (x.ErrorId != null && x.Error.IsPostMethod == model.IsPostMethod)) &&
                        (model.ErrorMessage.IsEmpty() || (x.ErrorId != null && x.Error.Message.Contains(model.ErrorMessage))) &&
                        (model.Username.IsEmpty() || (x.User != null && x.User.Username.Contains(model.Username))) &&
                        (!model.UserId.HasValue || (x.User != null && x.User.Id == model.UserId)));

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
                        await
                            items
                                .OrderBy(ordering.OrderByKeySelector)
                                .Skip(ordering.Skip)
                                .Take(ordering.Take)
                                .ToListAsync();
                }
                return
                    await
                        items
                            .OrderByDescending(ordering.OrderByKeySelector)
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

        public async Task<int?> CountAsync(LogSearchModel model)
        {
            if (model == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }
            model.Username = model.Username.TrimAndLower();

            var items =
                await
                    RepositoryContext.SearchAsync<Log>(x =>
                        (model.Action.IsEmpty() || x.Action.Contains(model.Action)) &&
                        (model.Area.IsEmpty() || x.Area.Contains(model.Area)) &&
                        (model.Controller.IsEmpty() || x.Controller.Contains(model.Controller)) &&
                        (model.ReferredHost.IsEmpty() || x.ReferredHost.Contains(model.ReferredHost)) &&
                        (!model.IsAjax.HasValue || x.IsAjax == model.IsAjax) &&
                        (model.FullUrl.IsEmpty() || (x.InfoId != null && x.Info.FullUrl.Contains(model.FullUrl))) &&
                        (model.ReferredUrl.IsEmpty() || (x.InfoId != null && x.Info.ReferredUrl.Contains(model.ReferredUrl))) &&
                        (model.UserAgent.IsEmpty() || (x.InfoId != null && x.Info.UserAgent.Contains(model.UserAgent))) &&
                        (model.Ip.IsEmpty() || (x.InfoId != null && x.Info.Ip.Contains(model.Ip))) &&
                        (model.Port.IsEmpty() || (x.InfoId != null && x.Info.Port.Contains(model.Port))) &&
                        (!model.IsPostMethod.HasValue || (x.ErrorId != null && x.Error.IsPostMethod == model.IsPostMethod)) &&
                        (model.ErrorMessage.IsEmpty() || (x.ErrorId != null && x.Error.Message.Contains(model.ErrorMessage))) &&
                        (model.Username.IsEmpty() || (x.User != null && x.User.Username.Contains(model.Username))) &&
                        (!model.UserId.HasValue || (x.User != null && x.User.Id == model.UserId)));

            if (items == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return await items.CountAsync();
        }
    }
}
