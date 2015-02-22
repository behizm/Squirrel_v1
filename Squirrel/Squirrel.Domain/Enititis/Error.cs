using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Squirrel.Domain.Enititis
{
    [Table("Errors", Schema = "Site")]
    public class Error
    {
        public Error()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public bool IsPostMethod { get; set; }

        public string Message { get; set; }
    }
}
