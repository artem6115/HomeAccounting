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

        public InventoryService(ILogger<InventoryService> log, IInventoryRepository rep, ITransactionRepository transactionRep)
        {
            InventoryRepository = rep;
            Logger = log;
            TransactionRepository = transactionRep;
        }
        public async Task<Inventory> Add(Inventory inventory)
        {
            var SumTransactionAfterDate = await TransactionRepository.GetBalanceAfterDate(inventory.AccountId, inventory.Date);
            var oldBalance = await InventoryRepository.GetLastBalance(inventory.AccountId);
            inventory.Value = oldBalance + SumTransactionAfterDate;
            var entity = await InventoryRepository.Add(inventory);
            return entity;
        }
        public void Delete(long Id) => InventoryRepository.Delete(Id);
        public async Task<List<Inventory>> GetInvByAccount(long accountId)
            =>await InventoryRepository.GetAccountInventories(accountId);

    }
}
