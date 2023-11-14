using DataLayer.Models;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DataLayer.Entities;
using DataLayer.Enum;
using System.Text.RegularExpressions;

namespace DataLayer.Repositories
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly AccountingDbContext Context;
        private readonly ILogger<StatisticRepository> _Log;
        public StatisticRepository(AccountingDbContext context, ILogger<StatisticRepository> logger)
        {
            Context = context;
            _Log = logger;
        }
        public async Task<IEnumerable<StatisticData>> BuildCategoriesStatistic(StatisticFilter filter)
        {

            IQueryable<Transaction> Query = Context.Transactions;

            if (!filter.AllTime)
            {
                DateTime StartDate = new DateTime(filter.Year, filter.Month, 1);
                DateTime FinishDate = new DateTime(filter.Year, filter.Month, 1,23,59,59).AddMonths(1).AddDays(-1);
                Query = Query.Where(x => x.Date >= StartDate && x.Date <= FinishDate);
            }

            if (!filter.AllAccounts)
                Query = Query.Where(x=> x.AccountId == filter.AccountId);

            if(filter.TypeTransaction == TypeTransaction.Expense)
                Query = Query.Where(x => !x.IsIncome);
            else
                Query = Query.Where(x => x.IsIncome);

            var result =await Query
                .Include(x=>x.Category)
                .GroupBy(x => x.Category)
                .Select(x => new StatisticData()
                {
                    Description = (x.Key!=null)? x.Key.Name.ToString():"Прочее",
                    X = (int)x.Sum(x => x.Value)
                })
                .ToListAsync();

            return result;
        }
        public async Task<IEnumerable<StatisticData>> BuildTransactionStatistic(StatisticFilter filter)
        {
            DateTime StartDate;
            DateTime FinishDate;
            IQueryable<Transaction> Query = Context.Transactions;
            IQueryable<IGrouping<int,Transaction>> Group=null;
            if (!filter.AllAccounts)
                Query = Query.Where(x => x.AccountId == filter.AccountId);

            if (filter.TypeTransaction == TypeTransaction.Income)
                Query = Query.Where(x => x.IsIncome);
            else
                Query = Query.Where(x => !x.IsIncome);

            
            if (filter.TypeGroup== TypeGroup.Day )
            {
                StartDate = new DateTime(filter.Year, filter.Month, 1);
                FinishDate = new DateTime(filter.Year, filter.Month, 1).AddMonths(1).AddDays(-1);
                Query = Query.Where(x => x.Date >= StartDate && x.Date <= FinishDate);
                Group = Query.GroupBy(x => x.Date.Day);
            }

            else
            {
                StartDate = new DateTime(filter.Year,1,1);
                FinishDate = new DateTime(filter.Year+1, 1, 1,23,59,59).AddDays(-1);
                Query = Query.Where(x => x.Date >= StartDate && x.Date <= FinishDate);
                Group = Query.GroupBy(x => x.Date.Month);

            }


            var result = await Group
                .Select(x => new StatisticData()
                {
                    Y = (int)Math.Round(x.Sum(x => x.Value)),
                    X = x.Key

                })
                .ToListAsync();

            return result;
        }
        public async Task<IEnumerable<StatisticData>> BuildBalanceStatistic(StatisticFilter filter)
        {
            DateTime StartDate;
            DateTime FinishDate;
            IQueryable<Inventory> Query = Context.Inventories;
            IQueryable<IGrouping<int, Inventory>> Group = null;
            if (!filter.AllAccounts)
                Query = Query.Where(x => x.AccountId == filter.AccountId);

            StartDate = new DateTime(filter.Year, filter.Month, 1);
            FinishDate = new DateTime(filter.Year, filter.Month, 1).AddMonths(1).AddDays(-1);
            Query = Query.Where(x => x.Date >= StartDate && x.Date <= FinishDate);
            Group = Query.GroupBy(x => x.Date.Day);


            var result = await Group
                .Select(x => new StatisticData()
                {
                    Y = (int)Math.Round(x.Sum(x => x.Value)),
                    X = x.Key

                })
                .ToListAsync();

            return result;
        }


    }
}
