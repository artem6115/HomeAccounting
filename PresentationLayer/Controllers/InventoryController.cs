using AutoMapper;
using BusinessLayer.Services;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PresentationLayer.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PresentationLayer.Controllers
{
    [Authorize]

    public class InventoryController : Controller
    {
        private readonly InventoryService _inventoryService;
        private readonly ILogger<InventoryController> _log;
        private readonly IMapper _mapper;
        private readonly AccountService _accountService;

        public InventoryController(InventoryService inventoryService, AccountService accountService, ILogger<InventoryController> logger,IMapper mapper)
        {
            _inventoryService = inventoryService;
            _log = logger;
            _mapper = mapper;
            _accountService = accountService;

        }
        public bool CheckExistData(DateTime date) => !_inventoryService.CheckExistData(date);

        [HttpGet]
        public async Task<IActionResult> Get(long id, DateTime? date = null)
        {
            if (!date.HasValue) date = DateTime.Now;
            double CalculateValue = await _accountService.GetBalance(id, date.Value.AddMinutes(1).AddSeconds(-1));
            var model = new InventoryViewModel()
            {
                Inventories = await _inventoryService.GetInvByAccount(id),
                Account = await _accountService.Get(id),
                CalculateBalance = CalculateValue,
                Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}",
                InventoryEditModel = new InventoryEditModel()
                {
                    AccountId = id,
                    Date = new DateTime(date.Value.Year,date.Value.Month, date.Value.Day, date.Value.Hour, date.Value.Minute,0),
                    Value = CalculateValue.ToString()
                }
                

            };
            return View("Inventories", model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(InventoryEditModel inventory,[FromServices] ITransactionRepository _transactionRepository)
        {
            if (ModelState.IsValid)
            {
                var calaculateBalance = await _accountService.GetBalance(inventory.AccountId, inventory.Date);
                var balance = Math.Round(double.Parse(inventory.Value), 2);
                await _inventoryService.Add(_mapper.Map<Inventory>(inventory));
                TempData["Message"] = "Инвентаризация добавлена";
                TempData["MessageStyle"] = "alert-success";
                if (inventory.CreateBalanceTransaction)
                {     
                    if (calaculateBalance != balance)
                    {
                        var transaction = new Transaction() {
                            AccountId = inventory.AccountId,
                            Comment = "Корректирующая транзакция",
                            Date = inventory.Date,
                            IsIncome = calaculateBalance < balance,
                            Value = Math.Round(Math.Abs(calaculateBalance-balance) , 2)
                        };
                        _transactionRepository.Add(transaction);
                    }
                }
            }
            return RedirectToAction("Get",new {Id= inventory.AccountId }); 
        }
        public IActionResult Delete(long id,long accountId)
        {
            _inventoryService.Delete(id);
            TempData["Message"] = "Инвентаризация удалена";
            TempData["MessageStyle"] = "alert-danger";
            return RedirectToAction("Get", new { Id = accountId });

        }
    }
}
