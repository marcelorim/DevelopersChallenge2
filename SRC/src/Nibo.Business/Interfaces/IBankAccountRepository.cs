using Nibo.Business.Models;
using System;

namespace Nibo.Business.Interfaces
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {
        BankAccount GetByAccountBankId(string accountCode, Guid bankId);
    }
}
