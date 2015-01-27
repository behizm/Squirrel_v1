using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Squirrel.Domain.Resources;

namespace Squirrel.Domain.ViewModels
{
    public class ConfigAddModel
    {
        [DisplayName(@"کلید")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        public string Key { get; set; }

        [DisplayName(@"مقدار")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength", ErrorMessage = null)]
        public string Value { get; set; }

        [DisplayName(@"شرح")]
        [StringLength(200, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLength", ErrorMessage = null)]
        public string Description { get; set; }
    }

    public class ConfigEditModel : ConfigAddModel
    {
        public Guid Id { get; set; }
    }

    public class ConfigRemoveModel
    {
        public Guid Id { get; set; }
    }
}
