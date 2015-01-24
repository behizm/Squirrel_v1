using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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



        [StringLength(150), Required]
        public string Title { get; set; }

        public DateTime? EditDate { get; set; }

        public PostsOrdering PostsOrdering { get; set; }

        public int View { get; set; }

        public bool IsPublished { get; set; }



        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey("Owner")]
        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

    }

    public enum PostsOrdering
    {
        Newer = 0,
        Older = 1,
        Popular = 2,
        LastEdited = 3,
    }
}
