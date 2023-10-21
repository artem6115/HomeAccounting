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
        Account Add(Account account);
        void Edit(Account account);
        void Delete(Account account);
        Account Get(long id);
        IEnumerable<Account> GetAlly();

       

    }
}
