using AutoMapper;
using BusinessLayer.Services;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using DataLayer;
using Microsoft.AspNetCore.Http.Extensions;
using DataLayer.Repositories;

namespace PresentationLayer.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionService _transactionService;
        private readonly IInventoryRepository _inventoryRepository;


        private readonly ILogger<TransactionController> _log;
        private readonly IMapper _mapper;

        public TransactionController(ILogger<TransactionController> log, TransactionService trs, IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _transactionService = trs;
            _mapper = mapper;
            _log = log;
            _inventoryRepository=inventoryRepository;
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
                ? _mapper.Map<TransactionEditModel> (await _transactionService.Get(id.Value)):new TransactionEditModel() { Date=DateTime.Now});
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(TransactionEditModel model)
        {
            if (ModelState.IsValid)
            {
                double difference = 0;
                Transaction oldTrans=null!;
                difference = ((model.IsIncome) ? double.Parse(model.Value) : (-1 * double.Parse(model.Value)));

                if (!model.Id.HasValue)
                {
                    await _transactionService.Add(_mapper.Map<Transaction>(model));
                    TempData["Message"] = "Транзакция добавлена";
                }
                else
                {
                    oldTrans = await _transactionService.Get(model.Id.Value);

                    await _transactionService.Edit(_mapper.Map<Transaction>(model));
                    difference = Math.Round(difference-(oldTrans.IsIncome?oldTrans.Value:-oldTrans.Value), 2);
                    TempData["Message"] = "Транзакция отредактирована";
                }
                if(oldTrans == null || model.AccountId==oldTrans.AccountId)
                    _inventoryRepository.RebuildInventories(model.AccountId, model.Date, difference);
                else
                {
                    var value = double.Parse(model.Value);
                    _inventoryRepository.RebuildInventories(oldTrans.AccountId, oldTrans.Date,(oldTrans.IsIncome)?-oldTrans.Value:oldTrans.Value);
                    _inventoryRepository.RebuildInventories(model.AccountId, model.Date,(model.IsIncome)?value:-value );

                }
                TempData["MessageStyle"] = "alert-success";
                TempData["MessageColor"] = "green";

                return RedirectToAction("Get");
            }
            return View("TransactionEdit", model);
        }

        public async Task<IActionResult> Delete(long id)
        {
            var transaction = await _transactionService.Get(id);
            _transactionService.Delete(id);
            _inventoryRepository.RebuildInventories(
                transaction.AccountId,
                transaction.Date,
                (transaction.IsIncome)?-transaction.Value: transaction.Value
            );

            TempData["Message"] = "Транзакция удалена";
            TempData["MessageStyle"] = "alert-danger";
            TempData["MessageColor"] = "red";
            return RedirectToAction("Get");
        }




    }
}
