using System;
using System.ComponentModel.DataAnnotations;
using Squirrel.Domain.Resources;

namespace Squirrel.Domain.ViewModels
{
    public class CommentAddModel
    {
        [Display(Name = @"محتوا")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public string Body { get; set; }

        [Display(Name = @"نام")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength", ErrorMessage = null)]
        public string Name { get; set; }

        [Display(Name = @"آدرس ایمیل")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength", ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_RegularExperssion", ErrorMessage = null)]
        public string Email { get; set; }

        [Display(Name = @"تائید شده است؟")]
        public bool? IsConfirmed { get; set; }

        [Display(Name = @"خوانده شده است؟")]
        public bool? IsRead { get; set; }

        [Display(Name = @"پست")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public Guid? PostId { get; set; }

        [Display(Name = @"پدر")]
        public Guid? ParentId { get; set; }

        [Display(Name = @"نام کاربری")]
        public string Username { get; set; }
    }

    public class CommentAddPublicModel : CommentAddModel
    {
        [Display(Name = @"نام")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength", ErrorMessage = null)]
        public new string Name { get; set; }

        [Display(Name = @"عبارت امنیتی")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public string Captcha { get; set; }
    }

    public class CommentEditModel
    {
        public Guid Id { get; set; }

        [Display(Name = @"محتوا")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public string Body { get; set; }

        [Display(Name = @"تائید شده است؟")]
        public bool? IsConfirmed { get; set; }

        [Display(Name = @"نام کاربری")]
        public string Username { get; set; }
    }

    public class CommentRemoveModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
    }

    public class CommentMarkModel : CommentRemoveModel
    {
    }

    public class CommentConfirmModel : CommentRemoveModel
    {
        public bool IsConfirm { get; set; }
    }

    public class CommentSearchModel
    {
        public Guid? Id { get; set; }

        [Display(Name = @"محتوا")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public string Body { get; set; }

        [Display(Name = @"نام")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength", ErrorMessage = null)]
        public string Name { get; set; }

        [Display(Name = @"آدرس ایمیل")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength", ErrorMessage = null)]
        public string Email { get; set; }

        [Display(Name = @"تائید شده است؟")]
        public bool? IsConfirmed { get; set; }

        [Display(Name = @"خوانده شده است؟")]
        public bool? IsRead { get; set; }

        [Display(Name = @"پست")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public Guid? PostId { get; set; }

        [Display(Name = @"پدر")]
        public Guid? ParentId { get; set; }

        [Display(Name = @"نام کاربری")]
        public string PostAuthorUsername { get; set; }

        public Guid? PostAuthorUserId { get; set; }
    }
}
