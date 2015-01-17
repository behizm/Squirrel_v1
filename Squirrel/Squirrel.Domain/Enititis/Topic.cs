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

        public FirstPostType FirstPost { get; set; }



        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

    }

    public enum FirstPostType
    {
        Newer = 0,
        Older = 1,
        Popular = 2,
        LastEdited = 3,
    }
}
