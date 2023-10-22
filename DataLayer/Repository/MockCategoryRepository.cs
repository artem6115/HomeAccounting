using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Category> Add(Category category)
        {
            var entity =(await Context.Categories.AddAsync(category)).Entity;
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

        public async Task<Category> Get(long id)=>await Context.Categories.SingleAsync(x=>x.Id == id);

        public async Task<IEnumerable<Category>> GetAll() =>await Context.Categories.ToListAsync();
    }
}
