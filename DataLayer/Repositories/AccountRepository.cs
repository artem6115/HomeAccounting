﻿using DataLayer.Entities;
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
            account.Name = account.Name.Trim();
            var entity = (await Context.Accounts.AddAsync(account)).Entity;
            Log.LogDebug($"Добавлен новый счёт - {entity.Name}");
            await Context.SaveChangesAsync();
            return entity;
          }

        public async void Delete(long id)
        {
            var account = await Get(id);
            Context.Accounts.Remove(account);
            Log.LogDebug($"Удален счёт - {account.Name}");
            Context.SaveChangesAsync();

        }

        public void Edit(Account account)
        {
            account.Name = account.Name.Trim();
            var entity = Context.Accounts.Update(account).Entity;
            Log.LogDebug($"Название счёта изменено - {entity.Id}");
            Context.SaveChangesAsync();
        }

        public Task<Account> Get(long id) => Context.Accounts.SingleAsync(x => x.Id == id && x.UserId == UserContext.UserId);

        public  Task<List<Account>> GetAll() =>  Context.Accounts.Where(x=>x.UserId==UserContext.UserId).ToListAsync();
        public bool CheckExistName(string name)
             => Context.Accounts.Where(x=>x.UserId==UserContext.UserId).AsEnumerable().Any(x => x.Name.Equals(name.Trim(),StringComparison.OrdinalIgnoreCase));
        
    }
}
