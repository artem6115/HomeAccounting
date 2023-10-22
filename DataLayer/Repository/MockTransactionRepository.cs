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
    public class MockTransactionRepository : ITransactionRepository
    {
        public readonly AccountingDbContext Context;
        public readonly ILogger<IAccountRepository> Log;

        public MockTransactionRepository(AccountingDbContext context, ILogger<IAccountRepository> logger)
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

        public async Task<Transaction> Get(long id)=>await Context.Transactions.SingleAsync(x=>x.Id == id);

        public async Task<IEnumerable<Transaction>> GetAll() =>await Context.Transactions.ToListAsync();

        public async Task<IEnumerable<Transaction>> GetAllforAccount(Account account)
            =>await Context.Transactions.Where(x=>x.AccountId==account.Id).ToListAsync();
    }
}
