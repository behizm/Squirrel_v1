using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface ICategoryService : IBaseService
    {
        Task AddAsync(string name, string parentName, string description);
        Task AddAsync(CategoryAddModel model);
        Task ChangeNameAsync(string oldName, string newName);
        Task ChangeParentAsync(string name, string parentName);
        Task ChangeDescriptionAsync(string name, string description);
        Task UpdateAsync(CategoryEditModel model);
        Task DeleteAsync(Guid id);
        Task<Category> ReplaceAsync(CategoryReplaceModel model);
        Task<List<Category>> ChildsAsync(string name);
        Task<List<string>> ChildsNameAsync(string name);
        Task<Category> FindByIdAsync(Guid id);
        Task<Category> FindByNameAsync(string name);
        Task<List<Category>> SearchAsync(CategorySearchModel model, OrderingModel<Category> ordering);
        Task<int?> CountAsync(CategorySearchModel model);
        Task<List<Topic>> TopicsAsync(string categoryName, bool isFamilyGet, int skip = 0, int take = 10);
        Task<List<Topic>> PublishedTopicsAsync(string categoryName, bool isFamilyGet, int skip, int take);
        Task ChangeAvatarAsync(Guid categoryId, Guid fileId);
        Task<List<CategoryTreeModel>> FamilyTree();
        Task<List<CategorySimpleTreeModel>> SimpleFamilyTree();
    }
}