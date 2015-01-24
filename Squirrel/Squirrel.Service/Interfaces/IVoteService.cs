using System;
using System.Threading.Tasks;
using Squirrel.Domain.ResultModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface IVoteService
    {
        OperationResult Result { get; }

        Task LikeAsync(Guid postId, Guid userId);
        Task DisLikeAsync(Guid postId, Guid userId);
        Task<bool?> FindAsync(Guid postId, Guid userId);
    }
}