using System;
using System.Collections.Generic;
using Squirrel.Domain.Enititis;

namespace Squirrel.Domain.ViewModels
{
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
        public Guid? UserId { get; set; }
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
