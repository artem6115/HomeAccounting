﻿using DataLayer.Models;
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

            IQueryable<Transaction> Query = Context.Transactions.Where(x=>x.Account.UserId == UserContext.UserId);

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
            IQueryable<Transaction> Query = Context.Transactions.Where(x => x.Account.UserId == UserContext.UserId);

            IQueryable<IGrouping<int,Transaction>> Group=null;
            if (!filter.AllAccounts)
                Query = Query.Where(x => x.AccountId == filter.AccountId);

            if (filter.TypeTransaction == TypeTransaction.Income)
                Query = Query.Where(x => x.IsIncome);
            else
                Query = Query.Where(x => !x.IsIncome);

            
            if (filter.TypeGroup== TimeGroup.Day )
            {
                StartDate = new DateTime(filter.Year, filter.Month, 1);
                FinishDate = StartDate.AddMonths(1);
                Query = Query.Where(x => x.Date >= StartDate && x.Date < FinishDate);
                Group = Query.GroupBy(x => x.Date.Day);
            }

            else
            {
                StartDate = new DateTime(filter.Year,1,1);
                FinishDate = StartDate.AddYears(1);
                Query = Query.Where(x => x.Date >= StartDate && x.Date < FinishDate);
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

            DateTime StartDate = new DateTime(filter.Year,(filter.TypeGroup==TimeGroup.Day)?filter.Month:1,1);
            DateTime CurrentDate = StartDate;
            TimeGroup GroupByDay = filter.TypeGroup;
            bool IsGroupByDays = filter.TypeGroup == TimeGroup.Day;
            DateTime FinishDate = IsGroupByDays ? StartDate.AddMonths(1) : StartDate.AddYears(1);

            var queryInventories = Context.Inventories.Where(x => x.Date >= StartDate && x.Date < FinishDate && x.Account.UserId==UserContext.UserId);
            var queryTransactions = Context.Transactions.Where(x => x.Date >= StartDate && x.Date < FinishDate && x.Account.UserId==UserContext.UserId);

            if (!filter.AllAccounts)
            {
                queryInventories = queryInventories.Where(x => x.AccountId == filter.AccountId);
                queryTransactions = queryTransactions.Where(x => x.AccountId == filter.AccountId);

            }
            var Invetories = await queryInventories.ToListAsync();
            var Transaction =await queryTransactions.ToListAsync();

            double Balance = InitializeBalance((filter.AllAccounts)?null:filter.AccountId,StartDate, Invetories.Where(x => x.Date < StartDate).MaxBy(x => x.Date));


            while (CurrentDate < FinishDate)
            {
                DateTime endCurrentDate = IsGroupByDays? CurrentDate.AddDays(1):CurrentDate.AddMonths(1);

                var lastInv = Invetories.Where(x => x.Date < endCurrentDate).MaxBy(x => x.Date);
                var transactionSum = Transaction.Where(x => x.Date < endCurrentDate && x.Date > (lastInv?.Date ?? CurrentDate))
                       .Select(x => x.IsIncome ? x.Value : -x.Value)
                       .Sum();
                Balance += transactionSum + (lastInv?.Value ?? 0);

                result.Add(new StatisticData()
                {
                    X =  (IsGroupByDays) ? CurrentDate.Day : CurrentDate.Month,
                    Y = (int)Math.Round(Balance)
                });

                CurrentDate = endCurrentDate;
            }

            return result;
        }

        private double InitializeBalance(long? accountId,DateTime StartDate,Inventory? lastInventory = null)
        {
            double Balance = 0;
            if (lastInventory != null)
                Balance += lastInventory.Value;
            IQueryable<Transaction> query = Context.Transactions.Where(x => x.Account.UserId == UserContext.UserId);
            if (accountId.HasValue) query = query.Where(x => x.AccountId == accountId.Value);
            var sumOldTransaction = query.Where(x => x.Date > ((lastInventory != null) ? lastInventory.Date : new DateTime()) && x.Date < StartDate)?
                   .Select(x => (x.IsIncome) ? x.Value : -x.Value)
                   .Sum();
            if (sumOldTransaction.HasValue) Balance += sumOldTransaction.Value;
            return Balance;

        }
    }
}
