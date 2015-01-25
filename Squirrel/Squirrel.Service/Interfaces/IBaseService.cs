using Squirrel.Domain.ResultModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface IBaseService
    {
        OperationResult Result { get; } 
    }
}