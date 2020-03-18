using System;
using System.Collections.Generic;
using System.Text;

namespace Nibo.Business.Models
{
    public class Bank : BaseEntity
    {
        public int Code { get; set; }
        public string Name { get; set; }

        /* Constructor */
        public Bank() { }
        public Bank(int code, string name)
        {
            this.Code = code;
            this.Name = name;
        }

    }
}
