using System;
using System.ComponentModel.DataAnnotations;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;

namespace Squirrel.Domain.ViewModels
{
    public class ProfileSearchModel
    {
        public Guid? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }

    public class ProfileCreateModel
    {
        [Display(Name = @"نام")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(30, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        public string Firstname { get; set; }

        [Display(Name = @"نام خانوادگی")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        public string Lastname { get; set; }
    }

    public class ProfileEditModel : ProfileCreateModel
    {
        public Guid Id { get; set; }
    }
}
