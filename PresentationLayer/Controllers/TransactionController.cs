using AutoMapper;
using BusinessLayer.Services;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using DataLayer;
using Microsoft.AspNetCore.Http.Extensions;

namespace PresentationLayer.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionService _transactionService;

        private readonly ILogger<TransactionController> _log;
        private readonly IMapper _mapper;

        public TransactionController(ILogger<TransactionController> log, TransactionService trs,IMapper mapper)
        {
            _transactionService = trs;
            _mapper = mapper;
            _log = log;
        }
        [HttpGet]

        public async void AddRandomEntity(int k)
        {
            var rnd = new Random();
            for (int i = 0; i < k; i++)
            {
                var tr = new Transaction()
                {
                    AccountId = 51,
                    Comment = rnd.Next(0, 100000).ToString(),
                    Date = DateTime.Now,
                    IsIncome = (rnd.Next(0, 2) == 1),
                    Value = rnd.Next(100, 1000000)
                };
                await _transactionService.Add(tr);
            }
        }
        public async Task<IActionResult> Get([FromServices] AccountService _accountService, 
            [FromServices]CategoryService _categoryService, 
            Filter filter) {
            //AddRandomEntity(200);
            
            var QueryExecuted =await _transactionService.GetTransactionsWithFilterByPages(filter);
            TransactionViewModel model = new TransactionViewModel() {
                Accounts = await _accountService.GetAll(),
                Categories = await _categoryService.GetAll(),
                Filter = filter,
                Transactions = QueryExecuted.Transactions,
                NumberOfLastPage = QueryExecuted.PageCount,
                Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{nameof(Transaction)}",
                Sum = _transactionService.ParseValue(QueryExecuted.Sum)
                
            };




            return View("Transactions", model);
            }
        [HttpGet]
        public async Task<IActionResult> EditPage(long? id)
            => View("TransactionEdit",(id.HasValue)
                ? _mapper.Map<TransactionEditModel> (await _transactionService.Get(id.Value)):null);
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(TransactionEditModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.Id.HasValue)
                {
                    await _transactionService.Add(_mapper.Map<Transaction>(model));
                    TempData["Message"] = "Транзакция добавлена";


                }
                else
                {
                    _transactionService.Edit(_mapper.Map<Transaction>(model));
                    TempData["Message"] = "Транзакция отредактирована";


                }
                TempData["MessageStyle"] = "alert-success";
                TempData["MessageColor"] = "green";

                return RedirectToAction("Get");
            }
            return View("TransactionEdit", model);
        }

        public async Task<IActionResult> Delete(long id)
        {
            _transactionService.Delete(id);
            TempData["Message"] = "Транзакция удалена";
            TempData["MessageStyle"] = "alert-danger";
            TempData["MessageColor"] = "red";
            return RedirectToAction("Get");
        }




    }
}
