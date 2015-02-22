using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Squirrel.Domain.Enititis
{
    [Table("LogsInfo", Schema = "Site")]
    public class LogInfo
    {
        public LogInfo()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        [Required]
        public string FullUrl { get; set; }

        public string ReferredUrl { get; set; }

        [Required, StringLength(250)]
        public string UserAgent { get; set; }

        [Required, StringLength(15)]
        public string Ip { get; set; }

        [Required, StringLength(5)]
        public string Port { get; set; }
    }
}
