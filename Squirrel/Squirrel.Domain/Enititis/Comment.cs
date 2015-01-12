using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



        public string Body { get; set; }



        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }

        [ForeignKey("Parent")]
        public Guid? ParentId { get; set; }
        public virtual Comment Parent { get; set; }
    }
}
