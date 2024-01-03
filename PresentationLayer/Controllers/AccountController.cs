
using BusinessLayer.Services;
using DataLayer;
using DataLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataLayer.Entities;
using PresentationLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace PresentationLayer.Controllers;

[Authorize]
public class AccountController : Controller
{ 
    private readonly AccountService accountService;
    private readonly InventoryService inventoryService;
    private readonly ILogger<AccountController> Log;
    private readonly IMapper _Mapper;

    public AccountController(ILogger<AccountController> log,  AccountService rep, IMapper mapper, InventoryService invService)
    {
        accountService = rep;
        _Mapper = mapper;
        Log = log;
        inventoryService = invService;
    }

    public  bool CheckExistName(string name)=> accountService.CheckExistName(name);

    [HttpGet]
    public async Task<IActionResult> Get() =>View("Accounts",await accountService.GetAllIncludeBalance());
    [HttpGet]
    public async Task<IActionResult> EditPage(long? id = null!)
    {
        if (id == null) return View("AccountEdit",null);
        var entity = await accountService.Get(id.Value);
        return  View("AccountEdit", _Mapper.Map<AccountEditModel>(entity));
     }

    

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Edit(AccountEditModel model)
    {
        if (ModelState.IsValid) {
            if (!model.Id.HasValue)
            {
                if (CheckExistName(model.Name))
                {
                    TempData["Error"] = "Счет с таким названием уже существует";
                    return RedirectToAction("EditPage",model.Id);
                }
                var account = _Mapper.Map<Account>(model);
                account.UserId =UserContext.UserId;
                account = await accountService.Add(account);
                TempData["Message"] = "Новый счет добавлен";
                await inventoryService.Add(
                    new Inventory(){
                        AccountId= account.Id,
                        Account=account,
                        Date = DateTime.Now.AddMinutes(-1),
                        Value=double.Parse(model.StartValue)
                    });
            }
            else {
                TempData["Message"] = "Название счета изменено";
                accountService.Edit(_Mapper.Map<Account>(model));
             }
           
            TempData["MessageStyle"] = "alert-success";
            return RedirectToAction("Get");
        }
        
        return View("AccountEdit",model);
    }
    [HttpGet]
    public async Task<IActionResult> Delete(long id) {
        accountService.Delete(id);
        TempData["Message"] = "Счёт удален";
        TempData["MessageStyle"] = "alert-danger";
        return RedirectToAction("Get");
    }
}
