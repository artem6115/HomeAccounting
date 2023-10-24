
using BusinessLayer.Services;
using DataLayer;
using DataLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataLayer.Entities;
using PresentationLayer.Models;
namespace PresentationLayer.Controllers;

public class AccountController : Controller
{ 
    private readonly AccountService accountService;
    private readonly ILogger<AccountController> Log;
    private readonly IMapper _Mapper;

    public AccountController(ILogger<AccountController> log,AccountService rep, IMapper maper)
    {
        accountService = rep;
        _Mapper=maper;
        Log = log;
    }

    public  bool CheckExistName(string name)=> !accountService.CheckExistName(name);

    [HttpGet]
    public async Task<IActionResult> Get() =>View("Accounts",await accountService.GetAll());
    [HttpGet]
    public async Task<IActionResult> GetToEdit(long? id = null!)
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
            //if (!model.Id.HasValue)await accountService.Add(_Mapper.Map<AccountEditModel,Account>(model));
            //else accountService.Edit(_Mapper.Map<AccountEditModel, Account>(model));
            if (!model.Id.HasValue) { 
                await accountService.Add(new Account() { Name = model.Name }); 
                }
            else accountService.Edit(new Account() { Name = model.Name, Id = model.Id.Value });
            TempData["Message"] = "Данные добавлены/отредактированы";
            return RedirectToAction("Get");
        }
        
        return View("AccountEdit",model);
    }
    [HttpGet]
    public async Task<IActionResult> Delete(long id) {
        accountService.Delete(id);
        TempData["Messange"] = "Счёт удален";
        return RedirectToAction("Get");
    }
}
