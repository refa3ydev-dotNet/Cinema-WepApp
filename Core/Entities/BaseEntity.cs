using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt{ get; set; }=DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
