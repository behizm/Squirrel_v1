using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace


namespace Squirrel.Service
{
    public interface ICommentService : IBaseService
    {
        Task AddAsync(CommentAddModel model);
        Task EditAsync(CommentEditModel model);
        Task DeleteAsync(CommentRemoveModel model);
        Task<Comment> FindByIdAsync(Guid id);
        Task<List<Comment>> SearchAsync<TKey>(CommentSearchModel model, OrderingModel<Comment, TKey> ordering);
        Task<int?> CountAsync(CommentSearchModel model);
        Task MarkAsRead(CommentMarkModel model);
        Task ChangeConfirmState(CommentConfirmModel model);
        Task<CommentStatisticsResultModel> Statistics(CommentStatisticsModel model);
        Task<List<Topic>> Topics(CommentSearchModel model);
    }
}
