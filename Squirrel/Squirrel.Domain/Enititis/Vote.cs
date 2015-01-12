using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Domain.Enititis
{
    public class Vote : BaseEntity
    {
        public Vote()
        {
        }

        public Vote(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
        }



        public bool Plus { get; set; }



        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
