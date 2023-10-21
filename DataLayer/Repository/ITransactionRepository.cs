using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ITransactionRepository
    {
       
        Transaction Add(Transaction account);
        void Edit(Transaction account);
        void Delete(Transaction account);
        Transaction Get(long id);
        IEnumerable<Transaction> GetAll();
        IEnumerable<Transaction> GetAllforAccount(Account account);
    }
}
