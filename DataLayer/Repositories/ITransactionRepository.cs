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
       
        Task<Transaction> Add(Transaction account);
        void Edit(Transaction account);
        void Delete(long id);
        Task<double> GetTransactionSum(long accountId, DateTime LastInvDate, DateTime CurrentDate);
        Task<Transaction> Get(long id);
        Task<List<Transaction>> GetAll();
        Task<List<Transaction>> GetAllforAccount(Account account);
        Task<List<Transaction>> GetByFilter(Filter filter);
        Task<QueryTransactionResult> GetTransactionsWithFilterByPages(Filter filter);
    }
}
