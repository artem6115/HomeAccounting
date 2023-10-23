using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AccountingDbContext Context;
        private readonly ILogger<IAccountRepository> Log;

        public TransactionRepository(AccountingDbContext context, ILogger<IAccountRepository> logger)
        {
            Context = context;
            Log = logger;
        }
        public async Task<Transaction> Add(Transaction transaction)
        {
            var entity =(await Context.Transactions.AddAsync(transaction)).Entity;
            Log.LogDebug($"Добавлена новая транзакция{entity.Id}");
            return transaction;
        }

        public void Delete(Transaction transaction)
        {
            Context.Transactions.Remove(transaction);
            Log.LogDebug($"Транзакция удалена{transaction.Id}");

        }

        public void Edit(Transaction transaction)
        {
            Context.Transactions.Update(transaction);
            Log.LogDebug($"Транзакция изменена{transaction.Id}");

        }

        public  Task<Transaction> Get(long id)=> Context.Transactions.SingleAsync(x=>x.Id == id);

        public  Task<List<Transaction>> GetAll() => Context.Transactions.ToListAsync();

        public  Task<List<Transaction>> GetAllforAccount(Account account)
            => Context.Transactions.Where(x=>x.AccountId==account.Id).ToListAsync();
    }
}
