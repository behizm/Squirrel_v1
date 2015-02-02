using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Description(@"تصویر")]
        Image = 0,

        [Description(@"فیلم")]
        Video = 1,

        [Description(@"صدا")]
        Audio = 2,

        [Description(@"آرشیو")]
        Archive = 3,

        [Description(@"مستندات")]
        Document = 4,

        [Description(@"برنامه")]
        Program = 5,
    }
}
