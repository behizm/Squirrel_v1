using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Domain.Enititis
{
    public class Profile : BaseEntity
    {
        public Profile()
        {
        }

        public Profile(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
        }



        public string Firstname { get; set; }
        public string Lastname { get; set; }

        

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("User")]
        public Guid? AvatarId { get; set; }
        public virtual File Avatar { get; set; }

    }
}
