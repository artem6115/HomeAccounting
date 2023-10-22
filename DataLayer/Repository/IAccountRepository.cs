using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IAccountRepository
    {
        Task<Account> Add(Account account);
        void Edit(Account account);
        void Delete(Account account);
        Task<Account> Get(long id);
        Task<IEnumerable<Account>> GetAlly();

       

    }
}
