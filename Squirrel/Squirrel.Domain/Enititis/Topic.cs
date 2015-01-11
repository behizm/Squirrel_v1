using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Squirrel.Domain.Enititis
{
    public class Topic : BaseEntity
    {
        public Topic()
        {
        }

        public Topic(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
        }



        public string Body { get; set; }
        public DateTime EditDate { get; set; }



        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

    }
}
