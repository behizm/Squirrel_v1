using System;
using System.ComponentModel.DataAnnotations;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;

namespace Squirrel.Domain.ViewModels
{
    public class UserCreateModel
    {
        [Display(Name = @"نام کاربری")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(25, MinimumLength = 5, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        [RegularExpression(@"[a-z0-9_]+", ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_RegularExperssion", ErrorMessage = null)]
        public string Username { get; set; }

        [Display(Name = @"پست الکترونیکی")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength", ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_RegularExperssion", ErrorMessage = null)]
        public string Email { get; set; }

        [Display(Name = @"کلمه عبور")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = @"تکرار کلمه عبور")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [Compare("Password", ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "UserCreateModel_ConfimPassword_Compare", ErrorMessage = null)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class UserUpdateModel
    {
        public Guid Id { get; set; }

        [Display(Name = @"نام کاربری")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(25, MinimumLength = 5, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        [RegularExpression(@"[a-z0-9_]+", ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_RegularExperssion", ErrorMessage = null)]
        public string Username { get; set; }

        [Display(Name = @"پست الکترونیکی")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength", ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_RegularExperssion", ErrorMessage = null)]
        public string Email { get; set; }

        [Display(Name = @"فعال است؟")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public bool IsActive { get; set; }

        [Display(Name = @"قفل است؟")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public bool IsLock { get; set; }

        [Display(Name = @"ادمین است؟")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public bool IsAdmin { get; set; }

        public static implicit operator UserUpdateModel(User user)
        {
            return new UserUpdateModel
            {
                Email = user.Email,
                Id = user.Id,
                IsActive = user.IsActive,
                IsAdmin = user.IsAdmin,
                IsLock = user.IsLock,
                Username = user.Username,
            };
        }
    }

    public class UserRemoveModel
    {
        public Guid Id { get; set; }
    }

    public class UserResetPasswordModel
    {
        public Guid Id { get; set; }

        [Display(Name = @"کلمه عبور")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        public string Password { get; set; }
    }

    public class UserSearchModel
    {
        public Guid? Id { get; set; }

        [Display(Name = @"نام کاربری")]
        public string Username { get; set; }

        [Display(Name = @"پست الکترونیکی")]
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }
        public DateTime? LastLoginFrom { get; set; }
        public DateTime? LastLoginTo { get; set; }
    }
}
