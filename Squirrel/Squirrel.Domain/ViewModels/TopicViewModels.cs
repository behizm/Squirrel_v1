using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;

namespace Squirrel.Domain.ViewModels
{
    public class TopicAddModel
    {
        [Display(Name = @"عنوان")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        [StringLength(150, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_StringLengthBound", ErrorMessage = null)]
        public string Title { get; set; }

        [Display(Name = @"ساختار مرتب سازی مطالب")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public PostsOrdering? PostsOrdering { get; set; }
        public PostsOrdering PostsOrdering2 { get; set; }

        [Display(Name = @"گروه")]
        [Required(ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_Required", ErrorMessage = null)]
        public Guid CategoryId { get; set; }

        public string Username { get; set; }
    }

    public class TopicEditModel : TopicAddModel
    {
        public Guid Id { get; set; }
    }

    public class TopicDeleteModel
    {
        public Guid Id { get; set; }
    }

    public class TopicSearchModel
    {
        public string Title { get; set; }
        public PostsOrdering? PostsOrdering { get; set; }
        public string Category { get; set; }
        public string Username { get; set; }
        public bool? IsPublished { get; set; }
    }
}
