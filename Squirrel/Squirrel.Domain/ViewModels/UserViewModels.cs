using System;
using System.ComponentModel.DataAnnotations;
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
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class UserDetailsModel
    {

    }

    public class UserSearchModel
    {
        public Guid? Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }
        public DateTime? LastLoginFrom { get; set; }
        public DateTime? LastLoginTo { get; set; }
    }
}
