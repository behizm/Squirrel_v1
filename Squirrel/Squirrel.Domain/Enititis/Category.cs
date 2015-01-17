using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Domain.Enititis
{
    public class Category : BaseEntity
    {
        public Category()
        {
        }

        public Category(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
        }


        [StringLength(50), Required]
        public string Name { get; set; }



        [ForeignKey("Parent")]
        public Guid? ParentId { get; set; }
        public virtual Category Parent { get; set; }

        [ForeignKey("Avatar")]
        public Guid? AvatarId { get; set; }
        public virtual File Avatar { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
