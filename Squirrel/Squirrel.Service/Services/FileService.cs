using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;
using Squirrel.Domain.ResultModels;
using Squirrel.Domain.ViewModels;
using File = Squirrel.Domain.Enititis.File;

namespace Squirrel.Service.Services
{
    class FileService : BaseService, IFileService
    {
        public List<FileTypeExtensions> FileTypeWithExtensionses { get; private set; }
        public List<FileTypeSize> FileTypeMaxSize { get; private set; }
        public List<string> ValidFileExtentions { get; private set; }
        public int TempSubDirectotyLifeTimeMinutes { get; private set; }

        public FileService()
        {
            FileTypeWithExtensionses = new List<FileTypeExtensions>
            {
                new FileTypeExtensions
                {
                    FileType = FileType.Image,
                    ExtensionsList = new List<string> {"bmp", "gif", "jpeg", "jpg", "png", "svg"}
                },
                new FileTypeExtensions
                {
                    FileType = FileType.Audio,
                    ExtensionsList = new List<string> {"mp3", "wave", "wma"}
                },
                new FileTypeExtensions
                {
                    FileType = FileType.Video,
                    ExtensionsList = new List<string> {"mpeg", "mpg", "mp4", "mov", "flv", "avi", "wmv"}
                },
                new FileTypeExtensions
                {
                    FileType = FileType.Archive,
                    ExtensionsList = new List<string> {"zip", "rar", "z7"}
                },
                new FileTypeExtensions
                {
                    FileType = FileType.Document,
                    ExtensionsList =
                        new List<string> {"xls", "xlsx", "pps", "ppt", "ppsx", "pptx", "doc", "docx", "txt"}
                },
                new FileTypeExtensions
                {
                    FileType = FileType.Program,
                    ExtensionsList = new List<string>()
                },
            };

            FileTypeMaxSize = new List<FileTypeSize>
            {
                new FileTypeSize {FileType = FileType.Archive, MaxSize = 40*1024*1024},
                new FileTypeSize {FileType = FileType.Audio, MaxSize = 10*1024*1024},
                new FileTypeSize {FileType = FileType.Document, MaxSize = 5*1024*1024},
                new FileTypeSize {FileType = FileType.Image, MaxSize = 3*1024*1024},
                new FileTypeSize {FileType = FileType.Program, MaxSize = 50*1024*1024},
                new FileTypeSize {FileType = FileType.Video, MaxSize = 60*1024*1024},
            };

            ValidFileExtentions = FileTypeWithExtensionses.SelectMany(x => x.ExtensionsList).ToList();
            TempSubDirectotyLifeTimeMinutes = 15;
        }

        public async Task AddAsync(FileAddModel file)
        {
            if (file == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_LackOfInputData);
                return;
            }

            if (string.IsNullOrEmpty(file.Username))
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            file.Username = file.Username.ToLower();
            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Username.ToLower() == file.Username);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }

            var item = new File
            {
                Address = file.FileAddress,
                Category = file.Category,
                Filename = file.Filename,
                IsPublic = !file.IsPublic.HasValue || file.IsPublic.Value,
                Name = file.Name,
                Size = file.Size,
                Type = file.Type.HasValue ? file.Type.Value : FileType.None,
                UserId = user.Id
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

        public async Task RemoveAsync(Guid fileId, Guid userId)
        {
            var item = await RepositoryContext.RetrieveAsync<File>(x => x.Id == fileId);
            if (item == null)
            {
                Result = OperationResult.Failed(ServiceMessages.FileService_FileNotFount);
                return;
            }

            var user = await RepositoryContext.RetrieveAsync<User>(x => x.Id == userId);
            if (user == null)
            {
                Result = OperationResult.Failed(ServiceMessages.UserService_UserNotFound);
                return;
            }
            if (!user.IsAdmin && item.UserId != userId)
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

        public async Task<List<File>> SearchAsync(FileSearchModel model, OrderingModel<File> ordering)
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

        public async Task<int?> CountAsync(FileSearchModel model)
        {
            var count =
                await RepositoryContext.CountAsync<File>(x =>
                    (!model.UserId.HasValue || x.IsPublic || x.UserId == model.UserId) &&
                    (string.IsNullOrEmpty(model.Category) || x.Category.Contains(model.Category)) &&
                    (string.IsNullOrEmpty(model.Filename) || x.Filename.Contains(model.Filename)) &&
                    (string.IsNullOrEmpty(model.Name) || x.Name.Contains(model.Name)) &&
                    (!model.CreateDateFrom.HasValue || x.CreateDate >= model.CreateDateFrom) &&
                    (!model.CreateDateTo.HasValue || x.CreateDate <= model.CreateDateTo) &&
                    (!model.SizeFrom.HasValue || x.Size >= model.SizeFrom) &&
                    (!model.SizeTo.HasValue || x.Size <= model.SizeTo) &&
                    (!model.Type.HasValue || x.Type == model.Type));

            if (count == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            return count;
        }

        public async Task<List<string>> Categories(string category, int skip, int take)
        {
            var items =
                await
                    RepositoryContext.SearchAsync<File>(
                        x => string.IsNullOrEmpty(category) || x.Category.Contains(category));

            if (items == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            try
            {
                Result = OperationResult.Success;
                return
                    await items.Select(x => x.Category).Distinct().OrderBy(x => x).Skip(skip).Take(take).ToListAsync();
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }
        }

        public FileType? GetFileTypeByExtention(string extension)
        {
            extension = extension.ToLower();
            var type = FileTypeWithExtensionses.FirstOrDefault(x => x.ExtensionsList.Any(e => e.ToLower() == extension));
            if (type == null)
            {
                Result = OperationResult.Failed(ServiceMessages.FileService_InvalidExtension);
                return null;
            }
            Result = OperationResult.Success;
            return type.FileType;
        }

        public FileType? GetFileTypeByFileName(string filename)
        {
            if (!filename.Split('.').Any())
            {
                Result = OperationResult.Failed(ServiceMessages.FileService_UnkhownExtension);
                return null;
            }

            var extension = filename.Split('.').Last();
            return GetFileTypeByExtention(extension);
        }

        public int? GetFileTypeSize(FileType fileType)
        {
            var item = FileTypeMaxSize.FirstOrDefault(x => x.FileType == fileType);
            if (item == null)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }
            Result = OperationResult.Success;
            return item.MaxSize;
        }

        public string CreateTempSubDirectory(string tempDirectotyPath)
        {
            try
            {
                if (!Directory.Exists(tempDirectotyPath))
                {
                    Directory.CreateDirectory(tempDirectotyPath);
                }
                else
                {
                    var dirInfo = new DirectoryInfo(tempDirectotyPath);
                    dirInfo.GetDirectories()
                        .Where(x => x.CreationTime < DateTime.Now.AddMinutes(-TempSubDirectotyLifeTimeMinutes))
                        .ForEach(x => Directory.Delete(x.FullName, true));
                }

                var folderName = Guid.NewGuid().ToString();
                var folderPath = string.Format("{0}\\{1}", tempDirectotyPath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                Result = OperationResult.Success;
                return folderName;
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }
        }

        public string MoveFromTempToMain(string sourcePath, string mainDirPath)
        {
            var folderName = Guid.NewGuid().ToString();
            var thisDayDir = string.Format("{0}{1}{2}",
                DateTime.Now.Year,
                DateTime.Now.Month.ToString("00"),
                DateTime.Now.Day.ToString("00"));
            var folderPath = string.Format("{0}\\{1}\\{2}", mainDirPath, thisDayDir, folderName);

            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            var sourceInfo = new FileInfo(sourcePath);
            var sourceDir = sourceInfo.DirectoryName;
            if (string.IsNullOrEmpty(sourceDir))
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            var targetPath = string.Format("{0}\\{1}\\{2}\\{3}", mainDirPath, thisDayDir, folderName, sourceInfo.Name);
            try
            {
                System.IO.File.Copy(sourcePath, targetPath, true);
            }
            catch (Exception)
            {
                Result = OperationResult.Failed(ServiceMessages.General_ErrorAccurred);
                return null;
            }

            Result = OperationResult.Success;
            var returnVal = string.Format("{0}/{1}/{2}", thisDayDir, folderName, sourceInfo.Name);
            try
            {
                Directory.Delete(sourceDir, true);
                return returnVal;
            }
            catch (Exception)
            {
                return returnVal;
            }
        }
    }
}
