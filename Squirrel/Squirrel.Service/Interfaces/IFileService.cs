﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.ViewModels;

// ReSharper disable once CheckNamespace
namespace Squirrel.Service
{
    public interface IFileService : IBaseService
    {
        List<FileTypeExtensions> FileTypeWithExtensionses { get; }
        List<string> ValidFileExtentions { get; }

        Task AddAsync(FileAddModel file);
        Task EditAsync(FileEditModel file);
        Task RemoveAsync(FileDeleteModel model);
        Task ReplaceAsync(FileReplaceModel model);
        Task<File> FindByIdAsync(Guid fileId);
        Task<List<File>> SearchAsync(FileSearchModel model, OrderingModel<File> ordering);
        Task<int?> CountAsync(FileSearchModel model);
        Task<List<string>> CategoriesAsync(string category, int skip, int take);
        FileType? GetFileTypeByExtention(string extension);
        FileType? GetFileTypeByFileName(string filename);
        int? GetFileTypeSize(FileType fileType);
        string CreateTempSubDirectory(string tempDirectotyPath);
        string MoveFromTempToMain(string sourcePath, string mainDirPath);
    }
}