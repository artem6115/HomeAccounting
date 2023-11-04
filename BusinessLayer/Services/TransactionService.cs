using DataLayer;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
namespace BusinessLayer.Services
{
    public class TransactionService
    {
        private readonly ILogger<AccountService> logger;
        private readonly ITransactionRepository Repository;
        public TransactionService(ILogger<AccountService> log, ITransactionRepository rep)
        {
            logger = log;
            Repository = rep;
        }
        public bool CheckValueHasMoreTwoNumber (double value)=> (value.ToString().Split(',')).Last().Length > 2;
        public Task<Transaction> Add(Transaction model) => Repository.Add(model);
        public void Edit(Transaction model) => Repository.Edit(model);
        public void Delete(long id) => Repository.Delete(id);
        public Task<List<Transaction>> GetAll() => Repository.GetAll();    
        public Task<List<Transaction>> GetByFilter(Filter filter) => Repository.GetByFilter(filter);

        public Task<Transaction> Get(long id) => Repository.Get(id);

        public Task<QueryTransactionResult> GetTransactionsWithFilterByPages(Filter filter) => Repository.GetTransactionsWithFilterByPages(filter);

        public string ParseValue(double sum)
        {
            string str = sum.ToString();
            var parts = str.Split(',');
            string doublePart = parts.Count()>1?parts.Last():"00";
            string MainPart = parts.First();
            int coutnPart = MainPart.Length % 3;
            var reversArrayNumbers = MainPart.ToCharArray().Reverse();
            var partsOfNumbers = reversArrayNumbers.Chunk(3);
            string result = "";
            foreach( var part in partsOfNumbers.Reverse())
                result += $"{string.Join("",part.Reverse())} ";
            return $"{result.Remove(result.Length - 1)},{doublePart}";
        }
    }
}
