using Nibo.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nibo.Business.Interfaces
{
    public interface IBankStatementRepository : IRepository<BankStatement>
    {
        Task<IEnumerable<BankTransactions>> GetTransactionsByStatement(Guid id);
    }
}
