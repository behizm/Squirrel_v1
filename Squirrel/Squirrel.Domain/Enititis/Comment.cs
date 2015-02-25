using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Squirrel.Domain.Enititis
{
    public class Comment : BaseEntity
    {
        public Comment()
        {
        }

        public Comment(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
        }


        [Required]
        public string Body { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public DateTime? EditeDate { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsReaded { get; set; }
        

        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }

        [ForeignKey("Parent")]
        public Guid? ParentId { get; set; }
        public virtual Comment Parent { get; set; }

        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
