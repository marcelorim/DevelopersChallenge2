using Nibo.Business.Interfaces;
using Nibo.Business.Models;
using Nibo.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nibo.Data.Repository
{
    public class BankAccountRepository : Repository<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(NiboDbContext context) : base(context) { }

        public BankAccount GetByAccountBankId(string accountCode, Guid bankId)
        {
            return Db.Accounts.Where(a => a.AccountCode == accountCode && a.BankId == bankId).FirstOrDefault();
        }
    }
}
