using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ResultModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface ITopicService
    {
        OperationResult Result { get; }

        Task AddAsync(Topic topic);
    }
}