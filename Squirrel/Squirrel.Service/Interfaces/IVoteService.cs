using System;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface IVoteService : IBaseService
    {
        Task LikeAsync(Guid postId, Guid userId);
        Task DisLikeAsync(Guid postId, Guid userId);
        Task<bool?> FindAsync(Guid postId, Guid userId);
    }
}