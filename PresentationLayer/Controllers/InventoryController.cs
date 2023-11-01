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
        public InventoryController(InventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _log = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            
            return View("Inventories", new InventoryViewModel()
               {
                   Inventories = await _inventoryService.GetInvByAccount(id),
                   AccountId = id,
               });;
        }
        [HttpPost]
        public async Task<IActionResult> Add(Inventory inventory)
        {
           await _inventoryService.Add(inventory);
            TempData["Message"] = "Инвентаризация добавлена";
            TempData["MessageStyle"] = "alert-success";
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
