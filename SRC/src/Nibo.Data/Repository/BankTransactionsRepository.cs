using Nibo.Business.Interfaces;
using Nibo.Business.Models;
using Nibo.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nibo.Data.Repository
{
    public class BankTransactionsRepository : Repository<BankTransactions>, IBankTransactionsRepository
    {
        public BankTransactionsRepository(NiboDbContext context) : base(context) { }
    }
}
