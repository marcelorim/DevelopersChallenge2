using System;
using System.Collections.Generic;

namespace Nibo.Business.Models
{
    public class BankStatement : BaseEntity
    {
        public DateTime InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public Guid BankAccountId { get; set; }

        /* EF Relation */
        BankStatementHeader Header { get; set; }
        public BankAccount BankAccount { get; set; }
        public IList<BankTransactions> Transactions { get; set; }

        /* Constructor */
        public BankStatement() { }

        public BankStatement(BankStatementHeader header, BankAccount bankAccount, DateTime initialDate, DateTime finalDate)
        {
            Init(header, bankAccount);

            this.InitialDate = initialDate;
            this.FinalDate = finalDate;
        }

        public BankStatement(BankStatementHeader header, BankAccount bankAccount)
        {
            Init(header, bankAccount);
        }

        private void Init(BankStatementHeader header, BankAccount bankAccount)
        {
            this.Header = header;
            this.BankAccount = bankAccount;
            this.BankAccountId = bankAccount.Id;
            this.Transactions = new List<BankTransactions>();
        }

        public void AddTransaction(BankTransactions transaction)
        {
            if (this.Transactions == null)
            {
                this.Transactions = new List<BankTransactions>();
            }
            this.Transactions.Add(transaction);
        }
    }
}
