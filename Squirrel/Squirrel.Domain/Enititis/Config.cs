using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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



        [StringLength(50), Required]
        public string Key { get; set; }

        [StringLength(100), Required]
        public string Value { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
