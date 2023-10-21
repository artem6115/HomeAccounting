using DataLayer.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class MockCategoryRepository: ICategoryRepository
    {
        public readonly AccountingDbContext Context;
        public readonly ILogger<IAccountRepository> Log;

        public MockCategoryRepository(AccountingDbContext context, ILogger<IAccountRepository> logger)
        {
            Context = context;
            Log = logger;
        }

        public Category Add(Category category)
        {
            var entity = Context.Categories.Add(category).Entity;
            Log.LogDebug($"Добавлена новая категория{entity.Id}");
            return entity;
        }

        public void Delete(Category category)
        {
            Context.Categories.Remove(category);
            Log.LogDebug($"Категория удалена{category.Id}");
        }

        public void Edit(Category category)
        {
            Context.Categories.Update(category);
            Log.LogDebug($"Категория периименована{category.Id}");

        }

        public Category Get(long id)=>Context.Categories.Single(x=>x.Id == id);

        public IEnumerable<Category> GetAll() => Context.Categories.ToList();
    }
}
