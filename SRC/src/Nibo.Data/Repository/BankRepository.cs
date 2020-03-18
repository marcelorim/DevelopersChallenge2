using Microsoft.EntityFrameworkCore;
using Nibo.Business.Interfaces;
using Nibo.Business.Models;
using Nibo.Data.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Nibo.Data.Repository
{
    public class BankRepository : Repository<Bank>, IBankRepository
    {
        public BankRepository(NiboDbContext context) : base(context) { }

        public Bank GetByAgencyCode(int agencyCode)
        {
            return Db.Banks.Where(ac => ac.Code == agencyCode).FirstOrDefault();
        }
    }
}
