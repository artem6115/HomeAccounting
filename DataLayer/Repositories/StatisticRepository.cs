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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataLayer.Repositories
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly AccountingDbContext Context;
        private readonly ILogger<StatisticRepository> _Log;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IInventoryRepository _inventoryRepository;



        public StatisticRepository(AccountingDbContext context,ITransactionRepository trRep, IInventoryRepository invRep, ILogger<StatisticRepository> logger)
        {
            Context = context;
            _Log = logger;
            _transactionRepository = trRep;
            _inventoryRepository = invRep;
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
            var result = new List<StatisticData>();

            #region Новый алгоритм
            var StartPeriod = new DateTime(filter.Year, filter.Month, 1);
            var EndPeriod = StartPeriod.AddMonths(1);

            var Transaction = Context.Transactions.Where(x => x.Date > StartPeriod && x.Date < EndPeriod).Where(x=>x.AccountId==filter.AccountId).ToList();
            var Invetories =await _inventoryRepository.GetAccountInventories(filter.AccountId);
            double balance = 0;
            var lastInventory = Invetories.Where(x=>x.Date < StartPeriod)?.MaxBy(x=>x.Date);
            if (lastInventory != null)
                balance += lastInventory.Value;
            var sumTrs = Transaction
            .Where(x => x.Date > ((lastInventory != null) ? lastInventory.Date : new DateTime()) && x.Date < StartPeriod)?
                   .Select(x => (x.IsIncome) ? x.Value : -x.Value)
                   .Sum();
            if (sumTrs.HasValue) balance += sumTrs.Value;

            for (DateTime date = StartPeriod; date.Day < DateTime.DaysInMonth(filter.Year,filter.Month); date = date.AddDays(1))
            {
                lastInventory = Invetories.Where(x => x.Date > date && x.Date < date.AddDays(1))?.MaxBy(x => x.Date);
                if (lastInventory != null) 
                    balance += lastInventory.Value; 
                sumTrs =  Transaction
                    .Where(x => x.Date >( (lastInventory!=null)?lastInventory.Date:date) && x.Date < date.AddDays(1))?
                    .Select(x=>(x.IsIncome)?x.Value: -x.Value)
                    .Sum();
                if(sumTrs.HasValue) balance += sumTrs.Value;
                result.Add(new StatisticData()
                {
                    X = date.Day,
                    Y = (int)Math.Round(balance)
                });

            }


            #endregion

            //#region Вариант 2
            //int MaxCount = 12;
            //int day = 1;
            //int month = filter.Month;
            //if (filter.TypeGroup == TypeGroup.Day)
            //    MaxCount = DateTime.DaysInMonth(filter.Year, filter.Month);

            //for (int i = 1; i <= MaxCount; i++)
            //{
            //    var StartDate = new DateTime(filter.Year, month, day);
            //    DateTime FinishDate;
            //    if (filter.TypeGroup == TypeGroup.Day)
            //        FinishDate = StartDate.AddDays(1).AddSeconds(-1);
            //    else
            //        FinishDate = StartDate.AddMonths(1).AddSeconds(-1);

            //    Inventory LastInventory = await _inventoryRepository.GetLastInventory(filter.AccountId, FinishDate);
            //    double balance;
            //    if (LastInventory != null && (LastInventory.Date > StartDate || i == 1))
            //        balance = LastInventory.Value + await _transactionRepository.GetTransactionSum(filter.AccountId, LastInventory.Date, FinishDate);
            //    else if (i != 1)
            //        balance = result.Last().Y + await _transactionRepository.GetTransactionSum(filter.AccountId, StartDate, FinishDate);
            //    else
            //        balance = await _transactionRepository.GetTransactionSum(filter.AccountId, new DateTime(), FinishDate);

            //    result.Add(new StatisticData()
            //    {
            //        X = i,
            //        Y = (int)Math.Round(balance)
            //    });
            //    if (filter.TypeGroup == TypeGroup.Day) day++;
            //    else month++;
            //}
            //#endregion

            return result;
        }


    }
}
