using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Domain.Enititis
{
    [Table("Configs", Schema = "Site")]
    public class Config : BaseEntity
    {
        public Config()
        {
        }

        public Config(Guid id, DateTime? createdate)
            : base(id, createdate)
        {
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
