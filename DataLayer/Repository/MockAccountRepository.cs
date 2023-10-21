using DataLayer.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class MockAccountRepository : IAccountRepository
    {
        public readonly AccountingDbContext Context;
        public readonly ILogger<IAccountRepository> Log;

        public MockAccountRepository(AccountingDbContext context, ILogger<IAccountRepository> logger)
        {
            Context = context;
            Log = logger;
        }
        public Account Add(Account account)
        {
            var entity = Context.Accounts.Add(account).Entity;
            Log.LogDebug($"Добавлен новый счёт {entity.Id}");
            return entity;
        }

        public void Delete(Account account)
        {
            Context.Accounts.Remove(account);
            Log.LogDebug($"Удален счёт {account.Id}");
        }

        public void Edit(Account account)
        {
            var entity = Context.Accounts.Update(account).Entity;
            Log.LogDebug($"Название счёта изменено {entity.Id}");
        }

        public Account Get(long id)=>Context.Accounts.Single(x=>x.Id==id);

        public IEnumerable<Account> GetAlly() => Context.Accounts.ToList();
    }
}
