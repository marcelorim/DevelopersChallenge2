using System;
using System.Collections.Generic;
using System.Text;

namespace Nibo.Business.Models
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
