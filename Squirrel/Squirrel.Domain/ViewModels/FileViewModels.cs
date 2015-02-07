using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;

namespace Squirrel.Domain.ViewModels
{
    public class FileAddModel
    {
        [Display(Name = @"نام")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        public string Name { get; set; }

        [Display(Name = @"دسترسی")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public bool? IsPublic { get; set; }

        [Display(Name = @"دسته بندی")]
        public string Category { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "FileAddModel_FileAddress_Required", ErrorMessage = null)]
        public string FileAddress { get; set; }

        public string Filename { get; set; }
        public int Size { get; set; }
        public FileType? Type { get; set; }
        public string Username { get; set; }
    }

    public class FileEditModel
    {
        public Guid Id { get; set; }

        [Display(Name = @"نام")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        public string Name { get; set; }

        [Display(Name = @"دسترسی")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public bool? IsPublic { get; set; }

        [Display(Name = @"دسته بندی")]
        public string Category { get; set; }

        public string Username { get; set; }
    }

    public class FileDeleteModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FullFilePath { get; set; }
    }

    public class FileReplaceModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "FileAddModel_FileAddress_Required", ErrorMessage = null)]
        public string FileAddress { get; set; }

        public string Username { get; set; }

        public string OldFilePath { get; set; }

        public string NewFilePath { get; set; }
    }

    public class FileSearchModel
    {
        public Guid? Id { get; set; }
        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public int? SizeFrom { get; set; }
        public int? SizeTo { get; set; }
        public FileType? Type { get; set; }
        public string Category { get; set; }
        public bool? IsPublic { get; set; }
        public string Username { get; set; }
    }

    public class FileRequestModel
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public FileType Type { get; set; }
        public string TragetKey { get; set; }
        public int Page { get; set; }
        public bool IsFixedType { get; set; }
    }

    public class FileTypeExtensions
    {
        public FileType FileType { get; set; }
        public List<string> ExtensionsList { get; set; }
    }

    public class FileTypeSize
    {
        public FileType FileType { get; set; }
        public int MaxSize { get; set; }
    }
}
