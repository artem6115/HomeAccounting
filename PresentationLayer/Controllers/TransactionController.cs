using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Get()
        {
            return View("Transactions");
        }
    }
}
