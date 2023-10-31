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
        private readonly int MaxNoteOnPage = 40;

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
            if(query.Count==0) return (0,new List<Transaction>(0));
            return (query.Count,query[page].ToList());
         }


        public Task<List<Transaction>> GetAllforAccount(Account account)
            => Context.Transactions.Where(x=>x.AccountId==account.Id).Include(x => x.Account).Include(x => x.Category).ToListAsync();

        public async Task<List<Transaction>> GetByFilter(Filter filter)
        {
            IQueryable<Transaction> data = Context.Transactions;
            string value = filter.Value;
            if (!string.IsNullOrWhiteSpace(value))
            {
                char operation = '=';
                if (new char[]{'<','>','=' }.Any(x=>x == value[0]))
                { operation = value[0];
                    value = value.Remove(0, 1);
                }
                value = value.Replace('.', ',');
                if (double.TryParse(value, out var doubleValue))
                {
                    switch (operation)
                    {
                        case '=': data = data.Where(x => x.Value == doubleValue);break;
                        case '>': data = data.Where(x => x.Value >= doubleValue); break;
                        case '<': data = data.Where(x => x.Value <= doubleValue); break;
                    }
                }
            }
            if (filter.Date is not null)
                data = data.Where(x => x.Date== filter.Date);
            if (filter.AccountId is not null)
                data = data.Where(x => x.AccountId == filter.AccountId);
            if (filter.CategoryId is not null)
                data = data.Where(x => x.CategoryId == filter.CategoryId);
            if (filter.TypeTransaction != 0)
            {
                if(filter.TypeTransaction == 1)
                    data = data.Where(x => x.IsIncome == true);
                else
                    data = data.Where(x => x.IsIncome == false);
            }
            if(filter.StringToFind is not null)
                data = data.Where(x => x.Comment.Contains(filter.StringToFind));
            if(filter.PropetryForSorting is not null)
            {
                switch (filter.PropetryForSorting)
                {
                    case "data":data = (filter.IsForward) ? data.OrderBy(x => x.Date) : data.OrderByDescending(x => x.Date); break;
                    case "account": data = (filter.IsForward) ? data.OrderBy(x => x.Account) : data.OrderByDescending(x => x.Account); break;
                    case "category": data = (filter.IsForward) ? data.OrderBy(x => x.Category) : data.OrderByDescending(x => x.Category); break;
                    case "value": data = (filter.IsForward) ? data.OrderBy(x => x.Value) : data.OrderByDescending(x => x.Value); break;

                }
            }

            data = data.Include(x => x.Account).Include(x => x.Category);

            return await data.ToListAsync();
        }

        public async Task<(List<Transaction>, int)> GetTransactionsWithFilterByPages(Filter filter)
        {
            var allTransactionWithFilter = await GetByFilter(filter);
            var pages = allTransactionWithFilter.Chunk(MaxNoteOnPage).ToList();
            if (pages.Count() == 0) return (new List<Transaction>(0),0);
            return (pages[filter.PageNumber].ToList(),pages.Count());
        }
    }
}
