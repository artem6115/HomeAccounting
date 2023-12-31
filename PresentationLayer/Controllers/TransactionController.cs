using AutoMapper;
using BusinessLayer.Services;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Http.Extensions;
using DataLayer.Repositories;
using DataLayer.Models;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DataLayer.Repository;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly TransactionService _transactionService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;

        private readonly ILogger<TransactionController> _log;
        private readonly IMapper _mapper;

        public TransactionController(ILogger<TransactionController> log, TransactionService trs, 
            IInventoryRepository inventoryRepository, IMapper mapper,
            IAccountRepository accountRepository, ICategoryRepository categoryRepository)
        {
            _transactionService = trs;
            _mapper = mapper;
            _log = log;
            _inventoryRepository = inventoryRepository;
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
        }
        [HttpGet]

        public async Task<IActionResult> Get(Filter filter) {
            var QueryExecuted = await _transactionService.GetTransactionsWithFilterByPages(filter);
            TransactionViewModel model = new TransactionViewModel() {
                Accounts = await _accountRepository.GetAll(),
                Categories = await _categoryRepository.GetAll(),
                Filter = filter,
                Transactions = QueryExecuted.Transactions,
                NumberOfLastPage = QueryExecuted.PageCount,
                Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}",
                Sum = _transactionService.ParseValue(QueryExecuted.Sum)

            };
            HttpContext.Session.Set("url",Encoding.UTF8.GetBytes(HttpContext.Request.GetDisplayUrl()));
            return View("Transactions", model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteByFilter( Filter filter)
        {
            await _transactionService.DeleteByFilter(filter);
            return RedirectToAction("Get");
        }

        [HttpGet]
        public async Task<IActionResult> EditPage(long? id)
        {
            TransactionEditPageModel model = new();
            if (id.HasValue)
            {
                var entity =await _transactionService.Get(id.Value);
                model.EditModel = _mapper.Map<TransactionEditModel>(entity);
            }
            else
                model.EditModel = new TransactionEditModel() {Date = CurrentDate()};
            

            model.Accounts = await _accountRepository.GetAll();
            model.Categories = await _categoryRepository.GetAll();
            return View("TransactionEdit", model);
        }

        private DateTime CurrentDate()
        {
            var date = DateTime.Now;
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(TransactionEditPageModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.EditModel.Id.HasValue)
                {
                    await _transactionService.Add(_mapper.Map<Transaction>(model.EditModel));
                    TempData["Message"] = "Транзакция добавлена";
                }
                else
                {
                    await _transactionService.Edit(_mapper.Map<Transaction>(model.EditModel));
                    TempData["Message"] = "Транзакция отредактирована";
                }
                TempData["MessageStyle"] = "alert-success";
                TempData["MessageColor"] = "green";
                HttpContext.Session.TryGetValue("url", out var bytes_url);
                if (bytes_url != null)
                {
                    var url = Encoding.UTF8.GetString(bytes_url);
                    return Redirect(url);
                }
                return RedirectToAction("Get");
            }
            return RedirectToAction("EditPage",model.EditModel.Id);
        }

        public async Task<IActionResult> Delete(long id)
        {
            _transactionService.Delete(id);
            TempData["Message"] = "Транзакция удалена";
            TempData["MessageStyle"] = "alert-danger";
            TempData["MessageColor"] = "red";
            HttpContext.Session.TryGetValue("url", out var bytes_url);
            if (bytes_url != null)
            {
                var url = Encoding.UTF8.GetString(bytes_url);
                return Redirect(url);
            }
            return RedirectToAction("Get");

        }




    }
}

