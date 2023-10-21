using DataLayer.Entities;
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
        public Transaction Add(Transaction transaction)
        {
            var entity = Context.Transactions.Add(transaction).Entity;
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

        public Transaction Get(long id)=>Context.Transactions.Single(x=>x.Id == id);

        public IEnumerable<Transaction> GetAll() => Context.Transactions.ToList();

        public IEnumerable<Transaction> GetAllforAccount(Account account)
            =>Context.Transactions.Where(x=>x.AccountId==account.Id).ToList();
    }
}
