using System;

namespace Nibo.Business.Models
{
    public class BankTransactions : BaseEntity
    {
        public string Type { get; set; }

        public DateTime Date { get; set; }

        public decimal TransactionValue { get; set; }

        public string Description { get; set; }

        public long Checksum { get; set; }

        public Guid BankStatementId { get; set; }

        public BankStatement Statement { get; set; }
    }
}
