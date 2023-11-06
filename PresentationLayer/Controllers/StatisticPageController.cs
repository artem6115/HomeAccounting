using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class StatisticPageController : Controller
    {
        public IActionResult Statistic() => View();

    }
}
