using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Squirrel.Domain.Enititis;
using Squirrel.Domain.Resources;

namespace Squirrel.Domain.ViewModels
{
    public class PostAddSimpleModel
    {
        [AllowHtml]
        public string Body { get; set; }
        public Guid TopicId { get; set; }
        public string Username { get; set; }
    }

    public class PostAddModel : PostAddSimpleModel
    {
        public string Abstract { get; set; }
        public Guid? HeaderImageId { get; set; }
        public List<Guid> Attachments { get; set; }
        public List<string> Tags { get; set; }
    }

    public class PostEditModel : PostAddModel
    {
        public Guid Id { get; set; }
        public bool IsPublic { get; set; }
        public string FlatedTags { get; set; }
        public string FlatedAttachments { get; set; }

        [Display(Name = @"زمان انتشار")]
        [RegularExpression(@"\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}", ErrorMessageResourceType = typeof(ValidationErrors), ErrorMessageResourceName = "General_RegularExperssion", ErrorMessage = null)]
        public string PublishPersianDate { get; set; }

        public DateTime? PublishDateTime { get; set; }
    }

    public class PostRemoveModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
    }

    public class PostSearchModel
    {
        public string Abstract { get; set; }
        public string Body { get; set; }
        public bool? IsPublic { get; set; }
        public string Topic { get; set; }
        public string User { get; set; }
    }
}
