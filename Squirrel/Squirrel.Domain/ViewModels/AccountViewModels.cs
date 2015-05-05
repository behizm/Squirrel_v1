using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squirrel.Domain.Resources;

namespace Squirrel.Domain.ViewModels
{
    public class AccountLoginModel
    {
        [Display(Name = @"نام کاربری")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public string Username { get; set; }

        [Display(Name = @"کلمه عبور")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public string Password { get; set; }
    }
}
