using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

namespace Squirrel.Service.Services
{
    class CategoryService : BaseService, ICategoryService
    {
        public async Task AddAsync(string name, string parentName, string description)
        {
            name = name.Trim();
            parentName = parentName.Trim();
            description = description.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var category =
                await RepositoryContext.RetrieveAsync<Category>(x => x.Name.ToLower() == name.ToLower());
            if (category != null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryExisted);
                return;
            }

            var item = new Category
            {
                Name = name,
                Description = string.IsNullOrEmpty(description) ? null : description,
            };

            if (!string.IsNullOrEmpty(parentName))
            {
                var parent =
                    await
                        RepositoryContext.RetrieveAsync<Category>(x => x.Name.ToLower() == parentName.ToLower());
                if (parent == null)
                {
                    Result = OperationResult.Failed(ServiceMessages.CategoryService_ParentNotFount);
                    return;
                }
                item.ParentId = parent.Id;
            }

            await RepositoryContext.CreateAsync(item);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task ChangeNameAsync(string oldName, string newName)
        {
            oldName = oldName.Trim();
            newName = newName.Trim();

            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var task1 = FindByNameAsync(oldName);
            var task2 = FindByNameAsync(newName);

            var oldCategory = await task1;
            if (oldCategory == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                return;
            }

            var newCategory = await task2;
            if (newCategory != null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryExisted);
                return;
            }

            oldCategory.Name = newName;
            await UpdateAsync(oldCategory);
        }

        public async Task ChangeParentAsync(string name, string parentName)
        {
            name = name.Trim();
            parentName = parentName.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var item = await FindByNameAsync(name);
            if (item == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                return;
            }

            if (string.IsNullOrEmpty(parentName))
            {
                item.ParentId = null;
            }
            else
            {
                var parent = await FindByNameAsync(parentName);
                if (parent == null)
                {
                    Result = OperationResult.Failed(ServiceMessages.CategoryService_ParentNotFount);
                    return;
                }
                item.ParentId = parent.Id;
            }

            await UpdateAsync(item);
        }

        public async Task ChangeDescriptionAsync(string name, string description)
        {
            name = name.Trim();
            description = description.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var item = await FindByNameAsync(name);
            if (item == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                return;
            }

            item.Description = string.IsNullOrEmpty(description) ? null : description;
            await UpdateAsync(item);
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await FindByIdAsync(id);
            if (item == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                return;
            }

            var task1 = RepositoryContext.CountAsync<Topic>(x => x.CategoryId == id);
            var task2 = RepositoryContext.CountAsync<Category>(x => x.ParentId == id);

            var childCount = await task2;
            if (childCount == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }
            if (childCount > 0)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_DeleteAsync_HasChild);
                return;
            }

            var topicCount = await task1;
            if (topicCount == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }
            if (topicCount > 0)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_DeleteAsync_HasTopic);
                return;
            }

            await RepositoryContext.DeleteAsync(item);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task ReplaceAsync(string name, string with)
        {
            name = name.Trim();
            with = with.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(with))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            var task1 = WarehouseContext.RetrieveAsync<Category>(x => x.Name.ToLower() == name.ToLower());
            var task2 = WarehouseContext.RetrieveAsync<Category>(x => x.Name.ToLower() == with.ToLower());

            var category = await task1;
            if (category == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                return;
            }

            var categoryWith = await task2;
            if (categoryWith == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                return;
            }

            var childs = await WarehouseContext.SearchAsync<Category>(x => x.ParentId == category.Id);
            if (childs == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return;
            }

            childs.ForEach(c => c.ParentId = categoryWith.Id);
            category.Topics.ForEach(t => t.CategoryId = categoryWith.Id);
            WarehouseContext.Delete(category);
            await WarehouseContext.SaveChangesAsync();

            if (WarehouseContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task<List<Category>> ChildsAsync(string name)
        {
            name = name.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var category = await FindByNameAsync(name);
            if (category == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                return null;
            }

            var childs = await RepositoryContext.SearchAsync<Category>(x => x.ParentId == category.Id);
            if (childs == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return await childs.ToListAsync();
        }

        public async Task<List<string>> ChildsNameAsync(string name)
        {
            name = name.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var category = await FindByNameAsync(name);
            if (category == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                return null;
            }

            var childs = await RepositoryContext.SearchAsync<Category>(x => x.ParentId == category.Id);
            if (childs == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return await childs.Select(x => x.Name).ToListAsync();
        }

        public async Task<Category> FindByIdAsync(Guid id)
        {
            var item =
                await RepositoryContext.RetrieveAsync<Category>(x => x.Id == id);

            if (item == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                return null;
            }

            Result = OperationResult.Success;
            return item;
        }

        public async Task<Category> FindByNameAsync(string name)
        {
            name = name.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var item =
                await RepositoryContext.RetrieveAsync<Category>(x => x.Name.ToLower() == name.ToLower());

            if (item == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                return null;
            }

            Result = OperationResult.Success;
            return item;
        }

        public async Task<List<Category>> SearchAsync(CategorySearchModel model, OrderingModel<Category> ordering)
        {
            if (model == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            model.Name = model.Name.Trim();
            model.Parent = model.Parent.Trim();
            model.Description = model.Description.Trim();

            var items =
                await RepositoryContext.SearchAsync<Category>(x =>
                    (string.IsNullOrEmpty(model.Name) || x.Name.Contains(model.Name.ToLower())) &&
                    (string.IsNullOrEmpty(model.Parent) || x.Parent.Name.Contains(model.Parent.ToLower())) &&
                    (string.IsNullOrEmpty(model.Description) || x.Description.Contains(model.Description)));

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
                        await items.OrderBy(ordering.KeySelector).Skip(ordering.Skip).Take(ordering.Take).ToListAsync();
                }
                return
                        await items.OrderByDescending(ordering.KeySelector).Skip(ordering.Skip).Take(ordering.Take).ToListAsync();
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }
        }

        public async Task<int?> CountAsync(CategorySearchModel model)
        {
            if (model == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            model.Name = model.Name.Trim();
            model.Parent = model.Parent.Trim();
            model.Description = model.Description.Trim();

            var count =
                await RepositoryContext.CountAsync<Category>(x =>
                    (string.IsNullOrEmpty(model.Name) || x.Name.Contains(model.Name.ToLower())) &&
                    (string.IsNullOrEmpty(model.Parent) || x.Parent.Name.Contains(model.Parent.ToLower())) &&
                    (string.IsNullOrEmpty(model.Description) || x.Description.Contains(model.Description)));

            if (count == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return count;
        }

        public async Task<List<Topic>> TopicsAsync(string name, bool isFamilyGet, int skip, int take)
        {
            name = name.Trim();

            IQueryable<Topic> items;
            if (isFamilyGet)
            {
                var familyIds = await ChildsIdAsync(name, true);
                if (!Result.Succeeded)
                    return null;
                items = await RepositoryContext.SearchAsync<Topic>(x => familyIds.Contains(x.CategoryId));
            }
            else
            {
                var category = await FindByNameAsync(name);
                if (category == null)
                    return null;

                items = await RepositoryContext.SearchAsync<Topic>(x => x.CategoryId == category.Id);
            }

            if (items == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return await items.OrderByDescending(x => x.CreateDate).Skip(skip).Take(take).ToListAsync();
        }

        public async Task ChangeAvatarAsync(Guid categoryId, Guid fileId)
        {
            var category = await FindByIdAsync(categoryId);
            if (category == null)
                return;

            var avatar = await RepositoryContext.RetrieveAsync<File>(x => x.Id == fileId);
            if (avatar == null)
            {
                Result = OperationResult.Failed(ServiceMessages.FileService_FileNotFount);
                return;
            }

            category.AvatarId = avatar.Id;
            await UpdateAsync(category);
        }


        private async Task UpdateAsync(Category category)
        {
            await RepositoryContext.UpdateAsync(category);

            if (RepositoryContext.OperationResult.Succeeded)
                Result = OperationResult.Success;

            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        private async Task<List<Guid>> ChildsIdAsync(string name, bool withOrgin = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return null;
            }

            var category = await FindByNameAsync(name);
            if (category == null)
            {
                Result = OperationResult.Failed(ServiceMessages.CategoryService_CategoryNotFount);
                return null;
            }

            var childs = await RepositoryContext.SearchAsync<Category>(x => x.ParentId == category.Id);
            if (childs == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            var returnVal = new List<Guid>();
            if (withOrgin)
                returnVal.Add(category.Id);

            try
            {
                returnVal.AddRange(await childs.Select(x => x.Id).ToListAsync());
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return returnVal;
        }
    }
}
