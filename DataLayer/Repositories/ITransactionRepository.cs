﻿using DataLayer.Entities;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ITransactionRepository
    {
       
        Task<Transaction> Add(Transaction transaction);
        Task Edit(Transaction transaction);
        void Delete(long id);
        Task<double> GetTransactionSum(long accountId, DateTime StartInvDate, DateTime EndInvDate);
        Task<Transaction> Get(long id);
        Task<List<Transaction>> GetAll();
        Task<List<Transaction>> GetAllforAccount(Account account);
        Task<List<Transaction>> GetByFilter(Filter filter);
        Task<QueryTransactionResult> GetTransactionsWithFilterByPages(Filter filter);
        public Task<List<Transaction>> DeleteByFilter(Filter filter);
    }
}
