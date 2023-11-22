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
    //Класс который генерирует записи в базе данных
    public class GeneratorEntitiesService
    {
        private readonly ILogger<GeneratorEntitiesService> _logger;
        private readonly IAccountRepository _accountRep;
        private readonly ICategoryRepository _categoryRep;
        private readonly ITransactionRepository _transactionRep;
        private readonly InventoryService _inventorySer;

        public GeneratorEntitiesService(
            ILogger<GeneratorEntitiesService> log,
            IAccountRepository ar ,
            ICategoryRepository cr,
            ITransactionRepository tr,
            InventoryService ir) 
        {
            _logger = log;
            _accountRep = ar;
            _categoryRep = cr;
            _transactionRep = tr;
            _inventorySer = ir;
        }

        public void Generate(int CountOfAccount, int CountOfTransactionForEveryMonth, int CountOfCategory)
        {
            //Спикок счетов и категорий
            List<Account> accounts = new List<Account>(CountOfAccount);
            List<Category> categories = new List<Category>(CountOfCategory);
            Random rnd = new Random((int)DateTime.Now.Ticks / DateTime.Now.Second);

            //Удаление уже созданых сгенерированных данных
            DeleteOldTestEntities();
            
            //Создание счетов и категорий
            for(int i = 0;i< CountOfAccount; i++)
                accounts.Add(_accountRep.Add(new Account() { Name = $"test_{i}" }).Result);
            for (int i = 0; i < CountOfCategory; i++)
                categories.Add(_categoryRep.Add(new Category() { Name = $"test_{i}" }).Result);
            for (int m = 1; m <= 12; m++)
            {
                // Добовление транзакций для каждого месяца
                for (int i = 0; i < CountOfTransactionForEveryMonth; i++)
                {
                    Transaction transaction = new Transaction()
                    {
                        Date = new DateTime(2023, m, rnd.Next(1, 29), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59)),
                        IsIncome = rnd.Next(0, 2) == 1,
                        Comment = rnd.Next(0, 999999).ToString(),
                        Value = rnd.Next(0, 999999),
                        AccountId = accounts[rnd.Next(0,accounts.Count)].Id,
                        CategoryId = categories[rnd.Next(0, categories.Count)].Id

                    };
                    _transactionRep.Add(transaction);
                }
                // Добовление инвенторизации на 3 даты, каждого месяца, каждого счета
                foreach (var account in accounts)
                {
                    _inventorySer.Add(new Inventory()
                    {
                        AccountId = account.Id,
                        Date = new DateTime(2023,m,10)
                    });
                    _inventorySer.Add(new Inventory()
                    {
                        AccountId = account.Id,
                        Date = new DateTime(2023, m, 20)
                    });
                    _inventorySer.Add(new Inventory()
                    {
                        AccountId = account.Id,
                        Date = new DateTime(2023, m, 28)
                    });
                }

            }




        _logger.LogWarning("Данные сгенерированы");

        }
        //Удаление сгенерированых данных
        private void DeleteOldTestEntities()
        {
            var categories = _categoryRep.GetAll().Result;
            foreach (var category in categories)
            {
                if(category.Name.Length>4 && category.Name.Substring(0,4) == "test")
                {
                    _categoryRep.Delete(category.Id);
                }
            }
            var accounts = _accountRep.GetAll().Result;
            foreach (var account in accounts)
            {
                if (account.Name.Length > 4 && account.Name.Substring(0, 4) == "test")
                {
                    _accountRep.Delete(account.Id);
                }
            }
        }
    }
}
