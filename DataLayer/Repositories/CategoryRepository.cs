﻿using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer.Repository
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly AccountingDbContext Context;
        private readonly ILogger<IAccountRepository> Log;

        public CategoryRepository(AccountingDbContext context, ILogger<IAccountRepository> logger)
        {
            Context = context;
            Log = logger;
        }

        public async Task<Category> Add(Category category)
        {
            category.Name = category.Name.Trim();
            var entity =(await Context.Categories.AddAsync(category)).Entity;
            Log.LogDebug($"Добавлена новая категория - {entity.Name}");
            await Context.SaveChangesAsync();
            return entity;
        }

        public async void Delete(long id)
        {
            var category = await Get(id);
            Context.Categories.Remove(category);
            Log.LogDebug($"Категория удалена - {category.Name}");
            Context.SaveChangesAsync();

        }

        public void Edit(Category category)
        {
            category.Name = category.Name.Trim();
            Context.Categories.Update(category);
            Log.LogDebug($"Категория периименована - {category.Id}");
            Context.SaveChangesAsync();


        }

        public  Task<Category> Get(long id)=> Context.Categories.SingleAsync(x=>x.Id == id && x.UserId == UserContext.UserId);

        public  Task<List<Category>> GetAll() => Context.Categories.Where(x=>x.UserId==UserContext.UserId).ToListAsync();
        public bool CheckExistName (string name)
            => Context.Categories.Where(x => x.UserId == UserContext.UserId).AsEnumerable().Any(x => x.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
