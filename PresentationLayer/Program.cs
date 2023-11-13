using DataLayer;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Services;
using AutoMapper;
using PresentationLayer.Models;
using DataLayer.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AccountingDbContext>(
                option => {
                    option.EnableSensitiveDataLogging(true);
                    option.UseSqlite(builder.Configuration.GetConnectionString("ConnectionString"));
                    }
                );

            builder.Services.AddTransient<IAccountRepository, AccountRepository>();
            builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
            builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
            builder.Services.AddTransient<IInventoryRepository, InventoryRepository>();
            builder.Services.AddTransient<IStatisticRepository, StatisticRepository>();



            builder.Services.AddTransient<TransactionService>();
            builder.Services.AddTransient<CategoryService>();
            builder.Services.AddTransient<AccountService>();
            builder.Services.AddTransient<InventoryService>();


            builder.Services.AddAutoMapper(typeof(MappintProfile));
            builder.Configuration.AddJsonFile("Properties/launchSettings.json", true);
            builder.WebHost.ConfigureLogging((context,log) => {
                log.AddConsole();
                log.AddFile();
                log.AddConfiguration(context.Configuration.GetSection("Logging"));
            });
            builder.WebHost.ConfigureLogging(b => b.AddConsole());


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            }
            // app.UseExceptionHandler("/Error");
            //  app.UseStatusCodePagesWithRedirects("/Error");

           // app.UseExceptionHandler("/Home/Error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
            app.Logger.LogTrace("Start");
        }
    }
}