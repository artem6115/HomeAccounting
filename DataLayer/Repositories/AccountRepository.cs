using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountingDbContext Context;
        private readonly ILogger<IAccountRepository> Log;

        public AccountRepository(AccountingDbContext context, ILogger<IAccountRepository> logger)
        {
            Context = context;
            Log = logger;
        }
         public async Task<Account> Add(Account account)
          {
            var entity = (await Context.Accounts.AddAsync(account)).Entity;
            Log.LogDebug($"Добавлен новый счёт {entity.Name}");
            await Context.SaveChangesAsync();
            return entity;
          }

        public async void Delete(long id)
        {
            var account = await Get(id);
            Context.Accounts.Remove(account);
            Log.LogDebug($"Удален счёт {account.Name}");
            Context.SaveChangesAsync();

        }

        public void Edit(Account account)
        {
            var entity = Context.Accounts.Update(account).Entity;
            Log.LogDebug($"Название счёта изменено {entity.Name}");
            Context.SaveChangesAsync();
        }

        public Task<Account> Get(long id) => Context.Accounts.SingleAsync(x => x.Id == id);

        public  Task<List<Account>> GetAll() =>  Context.Accounts.ToListAsync();
        public Task<bool> CheckExistName (string name)=> Context.Accounts.AnyAsync(x=>x.Name == name);

    }
}
