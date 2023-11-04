using DataLayer.Entities;
using DataLayer.Repositories;
using DataLayer.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class InventoryService
    {
        private readonly ILogger<InventoryService> Logger;
        private readonly IInventoryRepository InventoryRepository;
        private readonly ITransactionRepository TransactionRepository;
        private readonly AccountService _accountService;


        public InventoryService(ILogger<InventoryService> log, IInventoryRepository rep, ITransactionRepository transactionRep,AccountService accServ)
        {
            InventoryRepository = rep;
            Logger = log;
            TransactionRepository = transactionRep;
            _accountService= accServ;
        }
        public async Task<Inventory> Add(Inventory inventory)
        {
            var oldBalance = await InventoryRepository.GetLastInventory(inventory.AccountId,inventory.Date) ?? new Inventory() { Date=new DateTime()};

            var SumTransactionAfterDate = await TransactionRepository.GetTransactionSum(inventory.AccountId, oldBalance.Date, inventory.Date);

            inventory.Value =await _accountService.GetBalance(inventory.AccountId);
            var entity = await InventoryRepository.Add(inventory);
            return entity;
        }
        public void Delete(long Id) => InventoryRepository.Delete(Id);
        public async Task<List<Inventory>> GetInvByAccount(long accountId)
        {
 
            return await InventoryRepository.GetAccountInventories(accountId);
        }
    }
}
