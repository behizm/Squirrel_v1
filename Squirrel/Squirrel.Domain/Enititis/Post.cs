﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Domain.Enititis
{
    public class Post : BaseEntity
    {
        public Post()
        {
        }

        public Post(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
        }



        [StringLength(300)]
        public string Abstract { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime? EditDate { get; set; }

        public bool IsPublic { get; set; }

        public DateTime? PublishDate { get; set; }


        [ForeignKey("Topic")]
        public Guid TopicId { get; set; }
        public virtual Topic Topic { get; set; }

        [ForeignKey("Author")]
        public Guid AuthorId { get; set; }
        public virtual User Author { get; set; }

        [ForeignKey("HeaderImage")]
        public Guid? HeaderImageId { get; set; }
        public virtual File HeaderImage { get; set; }

        public virtual ICollection<File> Attachments { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
