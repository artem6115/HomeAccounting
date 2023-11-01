﻿using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AccountingDbContext Context;
        private readonly ILogger<IInventoryRepository> Log;
        public InventoryRepository(ILogger<IInventoryRepository> log, AccountingDbContext context)
        {
            Context = context;
            Log = log;
        }
        public async Task<Inventory> Add(Inventory inventory)
        {
            var entity = (await Context.Inventories.AddAsync(inventory)).Entity;
            Context.SaveChangesAsync();
            Log.LogInformation($"Добавлена инвентаризация - {entity.Id}");
            return entity;
        }

        public async void Delete(long Id)
        {
            var entity =await Get (Id);
            Context.Inventories.Remove(entity);
            Context.SaveChangesAsync();
            Log.LogInformation($"Удалена инвентаризация - {entity.Id}");
        }

        public async Task<Inventory> Get(long Id)
            => await Context.Inventories.Include(x=>x.Account).SingleAsync(x=>x.Id==Id);


        public async Task<List<Inventory>> GetAccountInventories(long accountId)
        {
            if (await Context.Accounts.FindAsync(accountId) == null)
                throw new Exception("Доступ к счету которого не существует (InventoryRepository)");
            return await Context.Inventories.Where(x => x.AccountId == accountId).OrderByDescending(x => x.Date).Include(x => x.Account).ToListAsync();
         }

        public async Task<Inventory> GetLastBalance(long accountId,DateTime Date)
        {
            var listInv = await Context.Inventories.Where(x => x.AccountId == accountId && x.Date<Date).ToListAsync();
            if (listInv == null || listInv.Count==0) return null;
            var LastInv = listInv.MaxBy(x => x.Date);
            return LastInv;
        }
    }
}