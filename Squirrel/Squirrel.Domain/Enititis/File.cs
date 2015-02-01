using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Squirrel.Domain.Enititis
{
    public class File : BaseEntity
    {
        public File()
        {
        }

        public File(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
        }


        [StringLength(25), Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [StringLength(50), Required]
        public string Filename { get; set; }

        public int Size { get; set; }

        public DateTime? EditDate { get; set; }

        public FileType Type { get; set; }

        public string Category { get; set; }

        public bool IsPublic { get; set; }



        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [InverseProperty("Attachments")]
        public virtual ICollection<Post> Posts { get; set; }
    }


    public enum FileType
    {
        Image = 0,
        Video = 1,
        Audio = 2,
        Archive = 3,
        Document = 4,
        Program = 5,
    }
}
