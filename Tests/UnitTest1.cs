using DataLayer.Models;
using Microsoft.Extensions.Logging;
using PresentationLayer.Controllers;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var ApiController = new StatisticController(
                new BusinessLayer.Services.StatisticRepository(
                    new DataLayer.AccountingDbContext(),null));
            var model = new StatisticFilter() {
                AllTime = true,
                AllAccounts=true,
                TypeTransaction = DataLayer.Enum.TypeTransaction.Income
                };
            var result = ApiController.Category().Result;
            double sum = result.Sum(x => x.X);
            string log = "";
            foreach (var item in result)
            {
                log +=($"Category - {item.Description}, value - {Math.Round(item.X/sum*100)}%{Environment.NewLine}");
            }
            if (!File.Exists("TestLogCategory.txt"))
                File.Create("TestLogCategory.txt");
            File.WriteAllText("TestLogCategory.txt", log);
            Assert.Pass();
        }
        [Test]
        public void Test2()
        {
            var ApiController = new StatisticController(
                new BusinessLayer.Services.StatisticRepository(
                    new DataLayer.AccountingDbContext(), null));

            var result = ApiController.Transaction().Result;
            string log = "";
            foreach (var item in result)
            {
                log += ($"Day/Month - {item.X}, value - {item.Y} руб {Environment.NewLine}");
            }
            if (!File.Exists("TestLogTransaction.txt"))
                File.Create("TestLogTransaction.txt");
            File.WriteAllText("TestLogTransaction.txt", log);
            Assert.Pass();
        }
        [Test]
        public void Test3()
        {
            var ApiController = new StatisticController(
                new BusinessLayer.Services.StatisticRepository(
                    new DataLayer.AccountingDbContext(), null));

            var result = ApiController.Balance().Result;
            string log = "";
            foreach (var item in result)
            {
                log += ($"Day/Month - {item.X}, value - {item.Y} руб {Environment.NewLine}");
            }
            if (!File.Exists("TestLogBalance.txt"))
                File.Create("TestLogBalance.txt");
            File.WriteAllText("TestLogBalance.txt", log);
            Assert.Pass();
        }
    }
}