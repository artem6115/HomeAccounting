using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Repository;
using DataLayer.Entities;
using BusinessLayer.Models;

namespace BusinessLayer.Services
{
    public class AccountService
    {
        public readonly ILogger<AccountService> logger;
        public readonly IAccountRepository Repository;
        public AccountService(ILogger<AccountService> log,IAccountRepository rep)
        {
            logger = log;
            Repository = rep;
        }
        public async Task<Account> Add(AccountEditModel model)
        {
            var ExistName = await Repository.CheckExistName(model.Name);
            if (ExistName)
                throw new Exception("Счёт с таким название уже существует!");
            return await Repository.Add(new Account { Name = model.Name });
        }
        public void Edit(AccountEditModel model)
            => Repository.Edit(new Account() { Id=model.Id.Value,Name=model.Name});

        public void Delete(AccountEditModel model)
            => Repository.Delete(new Account() { Id = model.Id.Value, Name = model.Name });
        public async Task<IEnumerable<Account>> GetAll() => await Repository.GetAll();
        public async Task<Account> Get(long id) => await Repository.Get(id);

    }
}
