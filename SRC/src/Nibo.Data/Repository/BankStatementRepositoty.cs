using Microsoft.EntityFrameworkCore;
using Nibo.Business.Interfaces;
using Nibo.Business.Models;
using Nibo.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nibo.Data.Repository
{
    public class BankStatementRepositoty : Repository<BankStatement>, IBankStatementRepository
    {
        public BankStatementRepositoty(NiboDbContext context) : base(context) { }

        public async Task<IEnumerable<BankTransactions>> GetTransactionsByStatement(Guid id)
        {
            return (await Db.Statements.AsNoTracking()
                .Include(t => t.Transactions)
                .FirstOrDefaultAsync(s => s.Id == id)).Transactions;
        }


        //public async Task<IEnumerable<BankStatement>> GetTransactionsByStatement(Guid transactionId)
        //{
        //    return await Db.Statements.AsNoTracking().Include(a => a.Transactions).FirstOrDefaultAsync(s => s.Id == transactionId);
        //}
    }
}
