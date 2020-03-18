using Nibo.Business.Models;
using System.Threading.Tasks;

namespace Nibo.Business.Interfaces
{
    public interface IBankRepository : IRepository<Bank>
    {
        Bank GetByAgencyCode(int agencyCode);
    }
}
