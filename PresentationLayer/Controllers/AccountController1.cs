using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class AccountController1 : Controller
    {
        [HttpGet]
        public IActionResult Index(Filter filter = null!)
        {
            return View();
        }
    }
}
