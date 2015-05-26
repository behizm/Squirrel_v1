using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;
using Squirrel.Utility.Helpers;

namespace Squirrel.Service.Services
{
    class CommentService : BaseService, ICommentService
    {
        public async Task AddAsync(CommentAddModel model)
        {
            if (model == null || !model.PostId.HasValue)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            User user = null;
            if (!string.IsNullOrEmpty(model.Username) || !string.IsNullOrWhiteSpace(model.Username))
            {
                model.Username = model.Username.Trim().ToLower();
                user = await RepositoryContext.RetrieveAsync<User>(x => x.Username == model.Username);
            }

            var comment = new Comment
            {
                Body = model.Body,
                IsConfirmed = model.IsConfirmed.HasValue && model.IsConfirmed.Value,
                IsReaded = model.IsRead.HasValue && model.IsRead.Value,
                ParentId = model.ParentId,
                PostId = model.PostId.Value,
            };
            if (user == null)
            {
                comment.Email = model.Email;
                comment.Name = model.Name;
            }
            else
            {
                comment.UserId = user.Id;
            }

            await RepositoryContext.CreateAsync(comment);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(RepositoryContext.OperationResult.Errors);
        }

        public async Task EditAsync(CommentEditModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrWhiteSpace(model.Username))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }
            model.Username = model.Username.Trim().ToLower();

            var commentTask = RepositoryContext.RetrieveAsync<Comment>(x => x.Id == model.Id);
            var userTask = RepositoryContext2.RetrieveAsync<User>(x => x.Username == model.Username);

            var comment = await commentTask;
            if (comment == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CommentService_CommentNotFound);
                return;
            }

            var user = await userTask;
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            if (!user.IsAdmin && comment.UserId != user.Id &&
                comment.Post.Author.Id != user.Id && comment.Post.Topic.Owner.Id != user.Id)
            {
                Result = OperationResult.Failed(ServiceMessages.General_NoAccessForThisOp);
                return;
            }

            comment.Body = model.Body;
            comment.IsReaded = true;
            if (model.IsConfirmed.HasValue)
                comment.IsConfirmed = model.IsConfirmed.Value;

            await RepositoryContext.UpdateAsync(comment);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(RepositoryContext.OperationResult.Errors);
        }

        public async Task DeleteAsync(CommentRemoveModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrWhiteSpace(model.Username))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }
            model.Username = model.Username.Trim().ToLower();

            var commentTask = RepositoryContext.RetrieveAsync<Comment>(x => x.Id == model.Id);
            var userTask = RepositoryContext2.RetrieveAsync<User>(x => x.Username == model.Username);
            var childsTask = RepositoryContext3.SearchAsync<Comment>(x => x.ParentId == model.Id);

            var comment = await commentTask;
            if (comment == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CommentService_CommentNotFound);
                return;
            }

            var user = await userTask;
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            if (!user.IsAdmin && comment.UserId != user.Id &&
                comment.Post.Author.Id != user.Id && comment.Post.Topic.Owner.Id != user.Id)
            {
                Result = OperationResult.Failed(ServiceMessages.General_NoAccessForThisOp);
                return;
            }

            var childs = await childsTask;
            if (childs == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }
            if (childs.Any())
            {
                Result = OperationResult.Failed(ServiceMessages.CommentService_NotRemovable);
                return;
            }

            await RepositoryContext.DeleteAsync(comment);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(RepositoryContext.OperationResult.Errors);
        }

        public async Task<Comment> FindByIdAsync(Guid id)
        {
            var comment = await RepositoryContext.RetrieveAsync<Comment>(x => x.Id == id);
            if (comment == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CommentService_CommentNotFound);
                return null;
            }
            Result = OperationResult.Success;
            return comment;
        }

        public async Task<List<Comment>> SearchAsync<TKey>(CommentSearchModel model, OrderingModel<Comment, TKey> ordering)
        {
            if (model == null || ordering == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var items =
                await
                    RepositoryContext.SearchAsync(GetSearchExpression(model));

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

        public async Task<int?> CountAsync(CommentSearchModel model)
        {
            if (model == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var items =
                await
                    RepositoryContext.SearchAsync(GetSearchExpression(model));

            if (items == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return await items.CountAsync();
        }

        public async Task MarkAsRead(CommentMarkModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrWhiteSpace(model.Username))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }
            model.Username = model.Username.Trim().ToLower();

            var commentTask = RepositoryContext.RetrieveAsync<Comment>(x => x.Id == model.Id);
            var userTask = RepositoryContext2.RetrieveAsync<User>(x => x.Username == model.Username);

            var comment = await commentTask;
            if (comment == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CommentService_CommentNotFound);
                return;
            }

            var user = await userTask;
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            if (!user.IsAdmin && comment.UserId != user.Id &&
                comment.Post.Author.Id != user.Id && comment.Post.Topic.Owner.Id != user.Id)
            {
                Result = OperationResult.Failed(ServiceMessages.General_NoAccessForThisOp);
                return;
            }

            comment.IsReaded = true;
            comment.EditeDate = DateTime.Now;
            await RepositoryContext.UpdateAsync(comment);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(RepositoryContext.OperationResult.Errors);
        }

        public async Task ChangeConfirmState(CommentConfirmModel model)
        {
            if (model == null || model.Username.IsNothing())
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }
            model.Username = model.Username.Trim().ToLower();

            var commentTask = RepositoryContext.RetrieveAsync<Comment>(x => x.Id == model.Id);
            var userTask = RepositoryContext2.RetrieveAsync<User>(x => x.Username == model.Username);

            var comment = await commentTask;
            if (comment == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CommentService_CommentNotFound);
                return;
            }

            var user = await userTask;
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            if (!user.IsAdmin && comment.UserId != user.Id &&
                comment.Post.Author.Id != user.Id && comment.Post.Topic.Owner.Id != user.Id)
            {
                Result = OperationResult.Failed(ServiceMessages.General_NoAccessForThisOp);
                return;
            }

            comment.IsConfirmed = model.IsConfirm;
            comment.EditeDate = DateTime.Now;
            await RepositoryContext.UpdateAsync(comment);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(RepositoryContext.OperationResult.Errors);
        }

        public async Task<CommentStatisticsResultModel> Statistics(CommentStatisticsModel model)
        {
            var allCountTask =
                RepositoryContext.CountAsync<Comment>(c =>
                    c.Post.AuthorId == model.AuthorId || c.Post.Topic.OwnerId == model.AuthorId);

            var unreadCountTask =
                RepositoryContext2.CountAsync<Comment>(c => !c.IsReaded &&
                    (c.Post.AuthorId == model.AuthorId || c.Post.Topic.OwnerId == model.AuthorId));

            var unconfirmedCountTask =
                RepositoryContext3.CountAsync<Comment>(c => !c.IsConfirmed &&
                    (c.Post.AuthorId == model.AuthorId || c.Post.Topic.OwnerId == model.AuthorId));

            var allCount = await allCountTask;
            if (allCount == null)
            {
                Result = OperationResult.Failed(RepositoryContext.OperationResult.Errors);
                return null;
            }

            var unreadCount = await unreadCountTask;
            if (unreadCount == null)
            {
                Result = OperationResult.Failed(RepositoryContext2.OperationResult.Errors);
                return null;
            }

            var unconfirmedCount = await unconfirmedCountTask;
            if (unconfirmedCount == null)
            {
                Result = OperationResult.Failed(RepositoryContext3.OperationResult.Errors);
                return null;
            }

            Result = OperationResult.Success;
            return new CommentStatisticsResultModel
            {
                All = allCount.Value,
                Unconfirmed = unconfirmedCount.Value,
                Unread = unreadCount.Value,
            };
        }

        public async Task<List<Topic>> Topics(CommentSearchModel model)
        {
            var searchExpression = GetSearchExpression(model);
            var searchTask = RepositoryContext.SearchAsync(searchExpression);

            var searchResult = await searchTask;
            if (searchResult == null)
            {
                Result = OperationResult.Failed(RepositoryContext.OperationResult.Errors);
                return null;
            }

            try
            {
                Result = OperationResult.Success;
                return await searchResult.Select(c => c.Post.Topic).Distinct().ToListAsync();
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }
        }



        private static Expression<Func<Comment, bool>> GetSearchExpression(CommentSearchModel model)
        {
            model.AuthorName = model.AuthorName.IsNotNothing() ? model.AuthorName.TrimAndLower() : string.Empty;

            return
                x =>
                    (string.IsNullOrEmpty(model.Body) || x.Body.Contains(model.Body)) &&
                    (string.IsNullOrEmpty(model.Name) || x.Name.Contains(model.Name) ||
                     (x.User != null && x.User.Profile != null && x.User.Profile.Firstname.Contains(model.Name)) ||
                     (x.User != null && x.User.Profile != null && x.User.Profile.Lastname.Contains(model.Name))) &&
                    (string.IsNullOrEmpty(model.Email) || x.Email.Contains(model.Email) ||
                     (x.User != null && x.User.Email.Contains(model.Email))) &&
                    (!model.AuthorId.HasValue || x.Post.AuthorId == model.AuthorId || x.Post.Topic.OwnerId == model.AuthorId) &&
                    (string.IsNullOrEmpty(model.AuthorName) || x.Post.Author.Username == model.AuthorName || x.Post.Topic.Owner.Username == model.AuthorName) &&
                    (!model.IsConfirmed.HasValue || x.IsConfirmed == model.IsConfirmed) &&
                    (!model.IsRead.HasValue || x.IsReaded == model.IsRead) &&
                    (!model.ParentId.HasValue || x.ParentId == model.ParentId) &&
                    (!model.UserId.HasValue || x.UserId == model.UserId) &&
                    (!model.PostId.HasValue || x.PostId == model.PostId);
        }
    }
}
