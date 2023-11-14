using AutoMapper;
using BusinessLayer.Services;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
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
        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            
            return View("Inventories", new InventoryViewModel()
               {
                   Inventories = await _inventoryService.GetInvByAccount(id),
                   Account =await _accountService.Get(id),
               });
        }
        [HttpPost]
        public async Task<IActionResult> Add(InventoryEditModel inventory)
        {
            var x = HttpContext.Request.QueryString.Value;
            var y = HttpContext.Request.RouteValues;

            if (ModelState.IsValid)
            {
                await _inventoryService.Add(_mapper.Map<Inventory>(inventory));
                TempData["Message"] = "Инвентаризация добавлена";
                TempData["MessageStyle"] = "alert-success";
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
