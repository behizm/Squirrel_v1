using System;
using System.ComponentModel.DataAnnotations;

namespace Squirrel.Domain.Enititis
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }

        public BaseEntity(Guid id, DateTime? createDate)
        {
            Id = id;
            CreateDate = createDate.HasValue ? createDate.Value : DateTime.Now;
        }

        [Key]
        public Guid Id { get; private set; }
        public DateTime CreateDate { get; private set; }
    }
}
