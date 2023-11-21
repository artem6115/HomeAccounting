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
            var entity = await InventoryRepository.Add(inventory);
            return entity;
        }

        public bool CheckExistData(DateTime date) =>InventoryRepository.CheckExistData(date);

        public void Delete(long Id) => InventoryRepository.Delete(Id);
        public async Task<List<Inventory>> GetInvByAccount(long accountId)
        {
 
            return await InventoryRepository.GetAccountInventories(accountId);
        }
    }
}
