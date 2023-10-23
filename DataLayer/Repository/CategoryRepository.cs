﻿using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var entity =(await Context.Categories.AddAsync(category)).Entity;
            Log.LogDebug($"Добавлена новая категория{entity.Name}");
            return entity;
        }

        public void Delete(Category category)
        {
            Context.Categories.Remove(category);
            Log.LogDebug($"Категория удалена{category.Name}");
        }

        public void Edit(Category category)
        {
            Context.Categories.Update(category);
            Log.LogDebug($"Категория периименована{category.Name}");

        }

        public  Task<Category> Get(long id)=> Context.Categories.SingleAsync(x=>x.Id == id);

        public  Task<List<Category>> GetAll() => Context.Categories.ToListAsync();
        public Task<bool> CheckExistName (string name)=>Context.Categories.AnyAsync(x=>x.Name == name);
    }
}
