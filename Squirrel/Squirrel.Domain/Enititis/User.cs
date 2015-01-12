using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Domain.Enititis
{
    public class User : BaseEntity
    {
        public User()
        {
        }

        public User(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
        }



        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHashed { get; set; }
        public int AccessFailed { get; set; }



        [ForeignKey("Profile")]
        public Guid? ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
