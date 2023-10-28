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
            Log.LogDebug($"Добавлена новая транзакция - {entity.Id}");
            await Context.SaveChangesAsync();
            return transaction;
        }

        public async void Delete(long id)
        {
            var transaction = await Get(id);
            Context.Transactions.Remove(transaction);
            Log.LogDebug($"Транзакция удалена - {transaction.Id}");
            Context.SaveChangesAsync();

        }

        public void Edit(Transaction transaction)
        {
            Context.Transactions.Update(transaction);
            Log.LogDebug($"Транзакция изменена - {transaction.Id}");
            Context.SaveChangesAsync();

        }

        public  Task<Transaction> Get(long id)=> Context.Transactions.Include(x => x.Account).Include(x => x.Category).SingleAsync(x=>x.Id == id);

        public  Task<List<Transaction>> GetAll() => Context.Transactions.Include(x => x.Account).Include(x => x.Category).ToListAsync();
        public async Task<(int,List<Transaction>)> GetAllPartial(int page, int count)
        {
            var query =Context.Transactions.Include(x => x.Account).Include(x => x.Category).AsEnumerable().Chunk(count).ToList();

            return (query.Count,query[page].ToList());
         }


        public Task<List<Transaction>> GetAllforAccount(Account account)
            => Context.Transactions.Where(x=>x.AccountId==account.Id).Include(x => x.Account).Include(x => x.Category).ToListAsync();

        public async Task<List<Transaction>> GetByFilter(Filter filter)
        {
            IQueryable<Transaction> data = Context.Transactions;

            if (filter.Value is not null)
                data = data.Where(x => x.Value == filter.Value);
            if (filter.Date is not null)
                data = data.Where(x => x.Date == filter.Date);
            if (filter.AccountId is not null)
                data = data.Where(x => x.AccountId == filter.AccountId);
            if (filter.CategoryId is not null)
                data = data.Where(x => x.CategoryId == filter.CategoryId);
            Log.LogDebug("Фильтрация выполнена");
            if (filter.PropetryForSorting is null) return await data.Include(x => x.Account).Include(x => x.Category).ToListAsync();
            switch (filter.PropetryForSorting)
            {
                case "Value":data =  (filter.IsForward)? data.OrderBy(x=>x.Value):data.OrderByDescending(x => x.Value); break;
                case "Category": data = (filter.IsForward) ? data.OrderBy(x => x.Category.Name) : data.OrderByDescending(x => x.Category.Name); break;
                case "Date": data = (filter.IsForward) ? data.OrderBy(x => x.Date) : data.OrderByDescending(x => x.Date); break;
            }
            Log.LogDebug("Сортировка выполнена");

            return await data.Include(x=>x.Account).Include(x=>x.Category).ToListAsync();
        }
    }
}
