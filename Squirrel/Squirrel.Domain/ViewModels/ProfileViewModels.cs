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
        public string Username { get; set; }
    }

    public class ProfileModelOverrided
    {
        public ProfileModelOverrided()
            : this(false)
        {
        }

        public ProfileModelOverrided(bool isNullProfile)
        {
            IsNullProfile = isNullProfile;
        }


        public DateTime CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
        public File Avatar { get; set; }

        [Display(Name = @"نام")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(30, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        public string Firstname { get; set; }

        [Display(Name = @"نام خانوادگی")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        public string Lastname { get; set; }

        public bool IsNullProfile { get; private set; }



        public static implicit operator ProfileModelOverrided(Profile profile)
        {
            if (profile == null)
            {
                return new ProfileModelOverrided
                {
                    Avatar = null,
                    CreateDate = DateTime.Now,
                    EditDate = null,
                    Firstname = null,
                    Lastname = null,
                    IsNullProfile = true,
                };
            }

            return new ProfileModelOverrided
            {
                Avatar = profile.Avatar,
                CreateDate = profile.CreateDate,
                EditDate = profile.EditDate,
                Firstname = profile.Firstname,
                Lastname = profile.Lastname,
                IsNullProfile = false,
            };
        }
    }

    public class ProfileAvatarModel
    {
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_NoPictureChoiced", ErrorMessage = null)]
        public Guid? AvatarId { get; set; }
        public string Username { get; set; }
    }
}
