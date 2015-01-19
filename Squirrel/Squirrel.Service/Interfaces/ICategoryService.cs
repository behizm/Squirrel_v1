using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface ICategoryService
    {
        OperationResult Result { get; }

        Task AddAsync(string name, string parentName, string description);
        Task ChangeNameAsync(string oldName, string newName);
        Task ChangeParentAsync(string name, string parentName);
        Task ChangeDescriptionAsync(string name, string description);
        Task DeleteAsync(Guid id);
        Task ReplaceAsync(string name, string with);
        Task<List<Category>> ChildsAsync(string name);
        Task<List<string>> ChildsNameAsync(string name);
        Task<Category> FindByIdAsync(Guid id);
        Task<Category> FindByNameAsync(string name);
        Task<List<Category>> SearchAsync(CategorySearchModel model, int skip = 0, int take = 10);
        Task<int?> CountAsync(CategorySearchModel model);
        Task<List<Topic>> TopicsAsync(string name, bool isFamilyGet, int skip = 0, int take = 10);
        Task ChangeAvatarAsync(Guid categoryId, Guid fileId);
    }
}