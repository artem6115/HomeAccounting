using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class StatisticPageController : Controller
    {
        public IActionResult Statistic() => 
            View( new StatisticFilter() { Month = DateTime.Now.Month,Year=DateTime.Now.Year});

    }
}
