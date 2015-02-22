using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Squirrel.Domain.Enititis
{
    [Table("Logs", Schema = "Site")]
    public class Log : BaseEntity
    {
        public Log()
        {
        }

        public Log(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
        }


        [StringLength(50)]
        public string Area { get; set; }

        [Required, StringLength(50)]
        public string Controller { get; set; }

        [Required, StringLength(50)]
        public string Action { get; set; }

        [StringLength(50)]
        public string ReferredHost { get; set; }

        public bool IsAjax { get; set; }

        

        [ForeignKey("Info")]
        public Guid InfoId { get; set; }
        public virtual LogInfo Info { get; set; }


        [ForeignKey("Error")]
        public Guid? ErrorId { get; set; }
        public virtual Error Error { get; set; }


        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
