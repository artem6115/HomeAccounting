using BusinessLayer.Models;
using BusinessLayer.Services;
using DataLayer;
using DataLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataLayer.Entities;

namespace PresentationLayer.Controllers;

public class AccountController : Controller
{ 
    private readonly AccountService accountService;
    private readonly ILogger<AccountController> Log;
    private readonly IMapper Maper;

    public AccountController(ILogger<AccountController> log,AccountService rep, IMapper maper)
    {
        accountService = rep;
        Maper=maper;
        Log = log;
    }
    [HttpGet]
    public async Task<IActionResult> Index() =>View("Accounts",await accountService.GetAll());

    [HttpPost]

    public async Task<IActionResult> Edit(AccountEditModel model)
    {
        if (!ModelState.IsValid) {
            if (!model.Id.HasValue)await accountService.Add(Maper.Map<AccountEditModel,Account>(model));
            else accountService.Edit(Maper.Map<AccountEditModel, Account>(model));
            return View("Accounts", await accountService.GetAll());
        }
        TempData["Messange"] = "Не корректные данные";
        return View();
    }
    [HttpGet]
    public IActionResult Delete(long id) {
        accountService.Delete(id);
        TempData["Messange"] = "Счёт удален";
        return View();
    }
}
