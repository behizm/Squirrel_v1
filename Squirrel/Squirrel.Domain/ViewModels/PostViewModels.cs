using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squirrel.Domain.Enititis;

namespace Squirrel.Domain.ViewModels
{
    public class PostAddModel
    {
        public string Abstract { get; set; }
        public string Body { get; set; }
        public Guid TopicId { get; set; }
        public Guid UserId { get; set; }
        public Guid? HeaderImageId { get; set; }
        public List<Guid> Attachments { get; set; }
        public List<string> Tags { get; set; }
    }

    public class PostEditModel
    {
        public Guid Id { get; set; }
        public string Abstract { get; set; }
        public string Body { get; set; }
        public Guid TopicId { get; set; }
        public Guid? HeaderImageId { get; set; }
        public List<Guid> Attachments { get; set; }
        public List<string> Tags { get; set; }
    }

    public class PostRemoveModel
    {
        public Guid Id { get; set; }
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
