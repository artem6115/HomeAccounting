using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Repository;
using DataLayer.Entities;

namespace BusinessLayer.Services
{
    public class AccountService
    {
        private readonly ILogger<AccountService> logger;
        private readonly IAccountRepository Repository;
        public AccountService(ILogger<AccountService> log,IAccountRepository rep)
        {
            logger = log;
            Repository = rep;
        }
        public bool CheckExistName(string name)=> Repository.CheckExistName(name);
        public  Task<Account> Add(Account model)=> Repository.Add(model);
        public void Edit(Account model) => Repository.Edit(model);
        public void Delete(long id) => Repository.Delete(id);
        public Task<List<Account>> GetAll() => Repository.GetAll();
        public Task<Account> Get(long id) =>  Repository.Get(id);

    }
}
