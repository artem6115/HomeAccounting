using BusinessLayer.Services;
using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        private readonly SettingsService _settingsService;
        public SettingController(SettingsService service) => _settingsService = service;
        public async Task<IActionResult> Get(Settings setting)
        {
            TempData["url"] = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            return View("Settings", setting);
        }
        public async Task<IActionResult> Put(Settings settings) {
            var _newSettings = await _settingsService.Update(settings); 
            return RedirectToAction("Get", _newSettings);
        }


    }
}
