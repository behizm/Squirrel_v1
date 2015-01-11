using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Domain.Enititis
{
    public class Post : BaseEntity
    {
        public Post()
        {
        }

        public Post(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
        }


        public string Body { get; set; }
        public DateTime EditDate { get; set; }
        public bool IsPublic { get; set; }


        [ForeignKey("Topic")]
        public Guid TopicId { get; set; }
        public virtual Topic Topic { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
