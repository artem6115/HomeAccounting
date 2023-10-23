using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CategorySrevice
    {
        private readonly ILogger<AccountService> logger;
        private readonly ICategoryRepository Repository;
        public CategorySrevice(ILogger<AccountService> log, ICategoryRepository rep)
        {
            logger = log;
            Repository = rep;
        }
        public Task<bool> CheckExistName(string name) => Repository.CheckExistName(name);
        public Task<Category> Add(Category model) => Repository.Add(model);
        public void Edit(Category model) => Repository.Edit(model);
        public void Delete(Category model) => Repository.Delete(model);
        public Task<List<Category>> GetAll() => Repository.GetAll();
        public Task<Category> Get(long id) => Repository.Get(id);
    }
}
