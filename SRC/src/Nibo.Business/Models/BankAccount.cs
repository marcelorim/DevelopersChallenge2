using System;

namespace Nibo.Business.Models
{
    public class BankAccount : BaseEntity
    {
        public string Type { get; set; }
        public string AgencyCode { get; set; }
        public string AccountCode { get; set; }
        public Guid BankId { get; set; }

        /* EF Relation */
        public Bank Bank { get; set; }
    }
}
