using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Domain.Enititis
{
    [Table("Profiles", Schema = "Membership")]
    public class Profile
    {
        public Profile()
        {
            UserId = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }

        public Profile(Guid id, DateTime? createDate)
        {
            UserId = id;
            CreateDate = createDate.HasValue ? createDate.Value : DateTime.Now;
        }



        [Key, ForeignKey("User")]
        public Guid UserId { get; private set; }

        public DateTime CreateDate { get; private set; }

        [StringLength(30), Required]
        public string Firstname { get; set; }

        [StringLength(50), Required]
        public string Lastname { get; set; }

        public DateTime? EditDate { get; set; }


        public virtual User User { get; set; }

        [ForeignKey("Avatar")]
        public Guid? AvatarId { get; set; }
        public virtual File Avatar { get; set; }

    }
}
