using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;

namespace Squirrel.Domain.ViewModels
{
    public class PostAddSimpleModel
    {
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
