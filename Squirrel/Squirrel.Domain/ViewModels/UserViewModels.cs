﻿using System;
using System.ComponentModel.DataAnnotations;
using Squirrel.Domain.Resources;

namespace Squirrel.Domain.ViewModels
{
    public class UserCreateModel
    {
        [Display(Name = @"نام کاربری")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required")]
        [StringLength(25, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength")]
        public string Username { get; set; }

        [Display(Name = @"پست الکترونیکی")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength")]
        public string Email { get; set; }

        [Display(Name = @"کلمه عبور")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required")]
        [StringLength(20, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength")]
        public string Password { get; set; }

        [Display(Name = @"تکرار کلمه عبور")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required")]
        [Compare("Password", ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "UserCreateModel_ConfimPassword_Compare")]
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
