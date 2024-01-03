using BusinessLayer.Services;
using DataLayer;
using DataLayer.Models;
using DataLayer.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        private readonly SettingsService _settingsService;
        private readonly IAccountRepository _accountRepository;
        public EmailSenderService _sender;

        public SettingController(SettingsService service,EmailSenderService sender ,IAccountRepository accountRepository) {
            _settingsService = service;
            _accountRepository = accountRepository;
            _sender = sender;
            
        }
        public async Task<IActionResult> Get(SettingsViewModel setting)
            {
            var model = new SettingsViewModel()
            {
                Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}",
                settings = setting.settings,
                Accounts =await _accountRepository.GetAll()
            };
            
            return View("Settings", model);
        }
        // $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


        [HttpPost]
        public async Task<IActionResult> Put(Settings settings) {
            var _newSettings = await _settingsService.Update(settings); 
            return RedirectToAction("Get");
        }
        public IActionResult Send()
        {
            _sender.Send(UserContext.UserName, "This is good new!!!", "Hello");
            return RedirectToAction("Get");

        }


    }
}
