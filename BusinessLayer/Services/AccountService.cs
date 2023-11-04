using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Repository;
using DataLayer.Entities;
using DataLayer.Models;
using DataLayer.Repositories;

namespace BusinessLayer.Services
{
    public class AccountService
    {
        private readonly ILogger<AccountService> logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly TransactionService _transactionService;

        private readonly IInventoryRepository _inventoryRepository;


        public AccountService(
            ILogger<AccountService> log,
            IAccountRepository rep,
            ITransactionRepository trep,
            IInventoryRepository inventoryRepository,
            TransactionService trServ
            )
        {
            logger = log;
            _accountRepository = rep;
            _transactionRepository = trep;
            _inventoryRepository = inventoryRepository;
             _transactionService=trServ;
        }
        public bool CheckExistName(string name)=> _accountRepository.CheckExistName(name);
        public  Task<Account> Add(Account model)=> _accountRepository.Add(model);
        public void Edit(Account model) => _accountRepository.Edit(model);
        public void Delete(long id) => _accountRepository.Delete(id);
        public async Task<List<AccountViewModel>> GetAll()
        {
        
        var entities = await _accountRepository.GetAll();
        var ViewResult = new List<AccountViewModel>();
        foreach (var item in entities)
        {
            ViewResult.Add(new AccountViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Balance =_transactionService.ParseValue(await GetBalance(item.Id))
            });
        }
        return ViewResult;

        }
        public async Task<double> GetBalance(long id)
        {
            Inventory lastInv =await  _inventoryRepository.GetLastInventory(id, DateTime.Now);
            var sumTransactions =await  _transactionRepository.GetTransactionSum(id, ((lastInv!=null)?lastInv.Date:new DateTime()), DateTime.Now);
            return Math.Round(sumTransactions+((lastInv!=null)?lastInv.Value:0),2);
        }
        public Task<Account> Get(long id) => _accountRepository.Get(id);

    }
}
