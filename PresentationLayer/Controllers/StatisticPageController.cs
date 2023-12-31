using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize]

    public class StatisticPageController : Controller
    {
        public IActionResult Statistic()
        {
            ViewData["url"] = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/statistic";
            return View(new StatisticFilter() { Month = DateTime.Now.Month, Year = DateTime.Now.Year });
        }

    }
}
