using DataLayer.Models;
using DataLayer.Repositories;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using PresentationLayer.Controllers;
using OpenQA.Selenium;
namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        StatisticFilter model = new StatisticFilter()
        {
            AllTime = true,
            AllAccounts = true,
            TypeTransaction = DataLayer.Enum.TypeTransaction.Income
        };

        IWebDriver browser;
        IWebElement inputAccount,value,create,errorName;

        [SetUp]
        public void TestINIT()
        {
            //browser = new OpenQA.Selenium.IE.InternetExplorerDriver(@"C:\Users\mikov_6gmdl0l\source\repos\WindowsFormsApp1\UnitTestProject1");
            browser = new OpenQA.Selenium.Edge.EdgeDriver();//C:\Users\mikov_6gmdl0l\source\repos\WebApplication1\UnitTestProject1\bin\Debug
            browser.Navigate().GoToUrl("https://localhost:7177/Account/EditPage");
            Sigin();
            Task.Delay(3000).Wait();
            browser.Navigate().GoToUrl("https://localhost:7177/Account/EditPage");
            inputAccount = browser.FindElement(By.Id("Name"));
            value = browser.FindElement(By.Id("StartValue"));
            inputAccount = browser.FindElement(By.Id("Accept"));
            errorName = browser.FindElement(By.Id("Name-error"));
        }


        [Test]
        public void TestEmpty()
        {
            create.Click();
            Assert.NotNull(errorName);
            Assert.That(errorName.Text,Is.EqualTo("Название счета обязательное поле"));
        }
        private void Sigin() {
            var login = browser.FindElement(By.Id("Input_Email"));
            var password = browser.FindElement(By.Id("Input_Password"));
            var signin = browser.FindElement(By.Id("login-submit"));
            login.SendKeys("artem.mikov.2003@mail.ru");
            password.SendKeys("Pistolet6115");
            signin.Click();
        }

    }
}