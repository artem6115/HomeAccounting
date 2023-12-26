﻿using AutoMapper;
using BusinessLayer.Services;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Http.Extensions;
using DataLayer.Repositories;
using DataLayer.Models;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace PresentationLayer.Controllers
{
    [Authorize]
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
            _inventoryRepository = inventoryRepository;
        }
        [HttpGet]

        public async Task<IActionResult> Get([FromServices] AccountService _accountService,
            [FromServices] CategoryService _categoryService,
            Filter filter) {
            var QueryExecuted = await _transactionService.GetTransactionsWithFilterByPages(filter);
            TransactionViewModel model = new TransactionViewModel() {
                Accounts = await _accountService.GetAll(),
                Categories = await _categoryService.GetAll(),
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
            => View("TransactionEdit",(id.HasValue)
                ? _mapper.Map<TransactionEditModel> (await _transactionService.Get(id.Value)):new TransactionEditModel() { Date=CurrentDate()});

        private DateTime CurrentDate()
        {
            var date = DateTime.Now;
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
        }

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
                    await _transactionService.Edit(_mapper.Map<Transaction>(model));
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
            return View("TransactionEdit", model);
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

