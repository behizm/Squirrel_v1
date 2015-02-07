﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;

namespace Squirrel.Domain.ViewModels
{
    public class CategoryAddModel
    {
        [Display(Name = @"نام گروه")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        public string Name { get; set; }

        [Display(Name = @"نام پدر گروه")]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        public string Parent { get; set; }

        [Display(Name = @"توضیحات گروه")]
        [StringLength(300, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength", ErrorMessage = null)]
        public string Description { get; set; }
    }

    public class CategoryEditModel : CategoryAddModel
    {
        public Guid Id { get; set; }
    }

    public class CategoryRemoveModel
    {
        public Guid Id { get; set; }
    }

    public class CategoryReplaceModel
    {
        public Guid Id { get; set; }

        [Display(Name = @"گروه جایگزین")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        public string ReplaceName { get; set; }
    }

    public class CategorySearchModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Parent { get; set; }
    }

    public class CategoryAvatarModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = @"هیچ عکسی انتخاب نشده است.")]
        public Guid? FileId { get; set; }
    }

    public class SimpleCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string AvatarUrl { get; set; }
        public int TopicCount { get; set; }
        public int ChildTopicCount { get; set; }

        public static implicit operator SimpleCategory(Category category)
        {
            return new SimpleCategory
            {
                AvatarUrl = category.AvatarId == null ? null : category.Avatar.Address,
                Id = category.Id,
                Name = category.Name,
                ParentName = category.ParentId == null ? null : category.Parent.Name,
            };
        }
    }

    public class CategoryTreeModel
    {
        public SimpleCategory Node { get; set; }
        public List<CategoryTreeModel> Childs { get; set; }
    }
}
