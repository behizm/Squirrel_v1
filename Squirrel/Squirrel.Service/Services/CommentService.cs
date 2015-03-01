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
            if (model == null || model.Username.IsEmpty())
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



        private static Expression<Func<Comment, bool>> GetSearchExpression(CommentSearchModel model)
        {
            model.PostAuthorUsername = model.PostAuthorUsername.IsNotEmpty() ? model.PostAuthorUsername.TrimAndLower() : string.Empty;

            return
                x =>
                    (string.IsNullOrEmpty(model.Body) || x.Body.Contains(model.Body)) &&
                    (string.IsNullOrEmpty(model.Name) || x.Name.Contains(model.Name) ||
                     (x.User != null && x.User.Profile != null && x.User.Profile.Firstname.Contains(model.Name)) ||
                     (x.User != null && x.User.Profile != null && x.User.Profile.Lastname.Contains(model.Name))) &&
                    (string.IsNullOrEmpty(model.Email) || x.Email.Contains(model.Email) ||
                     (x.User != null && x.User.Email.Contains(model.Email))) &&
                    (!model.PostAuthorUserId.HasValue || x.Post.AuthorId == model.PostAuthorUserId) &&
                    (string.IsNullOrEmpty(model.PostAuthorUsername) || x.Post.Author.Username == model.PostAuthorUsername) &&
                    (!model.IsConfirmed.HasValue || x.IsConfirmed == model.IsConfirmed) &&
                    (!model.IsRead.HasValue || x.IsReaded == model.IsRead) &&
                    (!model.ParentId.HasValue || x.ParentId == model.ParentId) &&
                    (!model.PostId.HasValue || x.PostId == model.PostId);
        }
    }
}
