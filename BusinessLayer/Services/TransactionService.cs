using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using Aspose.Cells;
using Microsoft.Office.Interop.Word;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
//using Microsoft.Office.Interop.Excel;

namespace BusinessLayer.Services
{
    public class TransactionService
    {
        private readonly ILogger<AccountService> logger;
        private readonly ITransactionRepository Repository;
        public TransactionService(ILogger<AccountService> log, ITransactionRepository rep)
        {
            logger = log;
            Repository = rep;
        }
        public bool CheckValueHasMoreTwoNumber (double value)=> (value.ToString().Split(',')).Last().Length > 2;
        public Task<Transaction> Add(Transaction model) => Repository.Add(model);
        public async System.Threading.Tasks.Task Edit(Transaction model) =>await Repository.Edit(model);
        public void Delete(long id) => Repository.Delete(id);
        public Task<List<Transaction>> GetAll() => Repository.GetAll();    
        public Task<List<Transaction>> GetByFilter(DataLayer.Models.Filter filter) => Repository.GetByFilter(filter);

        public Task<Transaction> Get(long id) => Repository.Get(id);
        public async Task<List<Transaction>> DeleteByFilter(DataLayer.Models.Filter filter) =>await Repository.DeleteByFilter(filter);


        public Task<QueryTransactionResult> GetTransactionsWithFilterByPages(DataLayer.Models.Filter filter) => Repository.GetTransactionsWithFilterByPages(filter);

        public string ParseValue(double sum)
        {
            string str = sum.ToString();
            var parts = str.Split(',');
            string doublePart = parts.Count()>1?parts.Last():"00";
            string MainPart = parts.First();
            int coutnPart = MainPart.Length % 3;
            var reversArrayNumbers = MainPart.ToCharArray().Reverse();
            var partsOfNumbers = reversArrayNumbers.Chunk(3);
            string result = "";
            foreach( var part in partsOfNumbers.Reverse())
                result += $"{string.Join("",part.Reverse())} ";
            return $"{result.Remove(result.Length - 1)},{doublePart}";
        }

        public async Task<Stream> GetExcel(DataLayer.Models.Filter filter)
        {
            var data = await Repository.GetByFilter(filter);
            var book = new Workbook();
            book.Worksheets.Clear();
            book.Worksheets.Add("Транзакции");
            var sheet = book.Worksheets[0];
            int i = 2;
            sheet.Cells[$"A1"].PutValue("Счёт");
            sheet.Cells[$"B1"].PutValue("Категория");
            sheet.Cells[$"C1"].PutValue("Величина операции");
            sheet.Cells[$"D1"].PutValue("Коментарий");
            sheet.Cells[$"E1"].PutValue("Дата создания");
            foreach (var item in data)
            {
                sheet.Cells[$"A{i}"].PutValue(item.Account.Name);
                sheet.Cells[$"B{i}"].PutValue(item.Category?.Name);
                sheet.Cells[$"C{i}"].PutValue(item.IsIncome ? item.Value : -item.Value);
                sheet.Cells[$"D{i}"].PutValue(item.Comment);
                sheet.Cells[$"E{i++}"].PutValue(item.Date.ToString());

            }
            //book.Save("Transactions.xlsm", SaveFormat.Xlsm);
            logger.LogDebug("Excel файл создан");
            return book.SaveToStream();
            //var book = new Workbook();
            //book.Sheets.Delete();
            //book.Sheets.Add();
            //Worksheet sheet = book.Sheets[0];
            //sheet.Name = "Transaction";
            //var app = new Application();
            //app.save


        }
        public async void GetWord(DataLayer.Models.Filter filter)
        {
            var data = await Repository.GetByFilter(filter);
            var app = new Application();
            app.Documents.Add();
            var dock = app.Documents[0];
            dock.Sections.Add();
            dock.Sections[0].Range.Text="test";
            app.ActiveDocument.SaveAs("Trn.dock");
            dock.Close();
            app.Quit();
            

        }

    }
}
