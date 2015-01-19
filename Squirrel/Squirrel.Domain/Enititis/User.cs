using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Squirrel.Domain.Enititis
{
    [Table("Users", Schema = "Membership")]
    public class User : BaseEntity
    {
        public User()
        {
            IsAdmin = false;
        }

        public User(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
            IsAdmin = false;
        }



        [StringLength(25), Required]
        public string Username { get; set; }

        [StringLength(50), Required]
        public string Email { get; set; }

        [StringLength(100), Required]
        public string PasswordHash { get; set; }

        public int AccessFailed { get; set; }

        public bool IsActive { get; set; }

        public DateTime? EditeDate { get; set; }

        public DateTime? LastLogin { get; set; }

        public bool IsLock { get; set; }

        public DateTime? LockDate { get; set; }

        public bool IsAdmin { get; set; }



        public virtual Profile Profile { get; set; }
    }
}
