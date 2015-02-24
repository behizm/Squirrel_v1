using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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

            var items = await RepositoryContext.SearchAsync(GetSearchExpression(model));
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

            var count = await RepositoryContext.CountAsync(GetSearchExpression(model));
            if (count == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return count;
        }

        public async Task CleanAsync(LogCleanModel model)
        {
            if (model == null || !model.CleanDateFrom.HasValue || !model.CleanDateTo.HasValue)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var items =
                await
                    RepositoryContext.SearchAsync<Log>(
                        x => x.CreateDate >= model.CleanDateFrom && x.CreateDate <= model.CleanDateTo);

            if (items == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }

            await RepositoryContext.DeleteAsync(items.ToArray());
            if (!RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }
            Result = OperationResult.Success;
        }



        private static Expression<Func<Log, bool>> GetSearchExpression(LogSearchModel model)
        {
            return
                x =>
                    (string.IsNullOrEmpty(model.ActionProp) || x.Action.Contains(model.ActionProp)) &&
                    (string.IsNullOrEmpty(model.AreaProp) || (model.AreaProp == "#" ? x.Area == null : x.Area.Contains(model.AreaProp))) &&
                    (string.IsNullOrEmpty(model.ControllerProp) || x.Controller.Contains(model.ControllerProp)) &&
                    (string.IsNullOrEmpty(model.ReferredHost) || x.ReferredHost.Contains(model.ReferredHost)) &&
                    (!model.IsAjax.HasValue || x.IsAjax == model.IsAjax) &&
                    (string.IsNullOrEmpty(model.FullUrl) || (x.InfoId != null && x.Info.FullUrl.Contains(model.FullUrl))) &&
                    (string.IsNullOrEmpty(model.ReferredUrl) || (x.InfoId != null && x.Info.ReferredUrl.Contains(model.ReferredUrl))) &&
                    (string.IsNullOrEmpty(model.UserAgent) || (x.InfoId != null && x.Info.UserAgent.Contains(model.UserAgent))) &&
                    (string.IsNullOrEmpty(model.Ip) || (x.InfoId != null && x.Info.Ip.Contains(model.Ip))) &&
                    (string.IsNullOrEmpty(model.Port) || (x.InfoId != null && x.Info.Port.Contains(model.Port))) &&
                    (!model.IsPostMethod.HasValue || (x.ErrorId != null && x.Error.IsPostMethod == model.IsPostMethod)) &&
                    (!model.IsErrorLog.HasValue || (model.IsErrorLog.Value ? x.ErrorId != null : x.ErrorId == null)) &&
                    (string.IsNullOrEmpty(model.ErrorMessage) || (x.ErrorId != null && x.Error.Message.Contains(model.ErrorMessage))) &&
                    (string.IsNullOrEmpty(model.Username) || (x.User != null && x.User.Username.Contains(model.Username))) &&
                    (!model.UserId.HasValue || (x.User != null && x.User.Id == model.UserId)) &&
                    (!model.CreateDateFrom.HasValue || x.CreateDate >= model.CreateDateFrom) &&
                    (!model.CreateDateTo.HasValue || x.CreateDate <= model.CreateDateTo);
        }
    }
}
