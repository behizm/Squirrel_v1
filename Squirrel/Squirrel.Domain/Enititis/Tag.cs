using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Domain.Enititis
{
    public class Tag:BaseEntity
    {
        public Tag()
        {
        }

        public Tag(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
        }


        [StringLength(25), Required]
        public string Name { get; set; }


        
        public virtual ICollection<Post> Posts { get; set; }
    }
}
