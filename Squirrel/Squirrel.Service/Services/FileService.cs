using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Squirrel.Data;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;

namespace Squirrel.Service.Services
{
    class FileService : IFileService
    {
        private IRepositoryContext _repositoryContext;
        private IRepositoryContext RepositoryContext
        {
            get { return _repositoryContext ?? (_repositoryContext = DataIOC.Get<IRepositoryContext>()); }
        }

        public OperationResult Result { get; private set; }

        public async Task AddAsync(File file)
        {
            var item = new File
            {
                Address = file.Address,
                Category = file.Category,
                Filename = file.Filename,
                IsPublic = file.IsPublic,
                Name = file.Name,
                Size = file.Size,
                Type = file.Type,
                UserId = file.UserId,
            };

            await RepositoryContext.CreateAsync(item);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task EditAsync(File file)
        {
            var item = await RepositoryContext.RetrieveAsync<File>(x => x.Id == file.Id);
            if (item == null)
            {
                Result = OperationResult.Failed(ServiceMessages.FileService_FileNotFount);
                return;
            }

            item.Address = file.Address;
            item.Category = file.Category;
            item.Filename = file.Filename;
            item.IsPublic = file.IsPublic;
            item.Name = file.Name;
            item.Size = file.Size;
            item.Type = file.Type;
            item.UserId = file.UserId;
            item.EditDate = DateTime.Now;

            await RepositoryContext.UpdateAsync(item);
            if (RepositoryContext.OperationResult.Succeeded)
            {
                Result = OperationResult.Success;
                return;
            }
            Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
        }

        public async Task RemoveAsync(Guid fileId, Guid? userId)
        {
            var item = await RepositoryContext.RetrieveAsync<File>(x => x.Id == fileId);
            if (item == null)
            {
                Result = OperationResult.Failed(ServiceMessages.FileService_FileNotFount);
                return;
            }

            if (userId.HasValue && item.UserId != userId)
            {
                Result = OperationResult.Failed(ServiceMessages.FileService_NoAccess);
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

        public async Task<File> FindByIdAsync(Guid fileId)
        {
            var item = await RepositoryContext.RetrieveAsync<File>(x => x.Id == fileId);
            Result =
                item == null
                    ? OperationResult.Failed(ServiceMessages.FileService_FileNotFount)
                    : OperationResult.Success;
            return item;
        }

        public async Task<List<File>> SearchAsync(FileSearchModel model, int skip, int take)
        {
            var items =
                await RepositoryContext.SearchAsync<File>(x =>
                    (!model.UserId.HasValue || x.IsPublic || x.UserId == model.UserId) &&
                    (string.IsNullOrEmpty(model.Category) || x.Category.Contains(model.Category)) &&
                    (string.IsNullOrEmpty(model.Filename) || x.Filename.Contains(model.Filename)) &&
                    (string.IsNullOrEmpty(model.Name) || x.Name.Contains(model.Name)) &&
                    (!model.CreateDateFrom.HasValue || x.CreateDate >= model.CreateDateFrom) &&
                    (!model.CreateDateTo.HasValue || x.CreateDate <= model.CreateDateTo) &&
                    (!model.SizeFrom.HasValue || x.Size >= model.SizeFrom) &&
                    (!model.SizeTo.HasValue || x.Size <= model.SizeTo) &&
                    (!model.Type.HasValue || x.Type == model.Type));

            if (items == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return await items.OrderBy(x => x.Name).Skip(skip).Take(take).ToListAsync();
        }

    }
}
