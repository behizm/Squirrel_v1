using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace


namespace Squirrel.Service
{
    public interface IPostService
    {
        OperationResult Result { get; }

        Task AddAsync(PostAddModel model);
        Task EditAsync(PostEditModel model, Guid userId);
        Task DeleteAsync(PostRemoveModel model, Guid userId);
        Task<Post> FindByIdAsync(Guid id);
        Task<List<Post>> SearchAsync(PostSearchModel model, OrderingModel<Post> ordering);
        Task<int?> CountAsync(PostSearchModel model);
        Task PublicPostAsync(Guid id, Guid userId);
        Task PrivatePostAsync(Guid id, Guid userId);
    }
}